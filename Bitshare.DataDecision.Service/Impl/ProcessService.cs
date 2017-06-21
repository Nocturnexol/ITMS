using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Model;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Bitshare.PTMM.Common;
using Bitshare.PTMM.Service.Helper;
namespace Bitshare.PTMM.Service.Impl
{
    #region 工厂定义
    /// <summary>
    /// 流程管理的工厂类
    /// </summary>
    public class ProcessServiceFactory
    {
        /// <summary>
        /// 流程管理
        /// </summary>
        public ProcessServiceFactory()
        {
            //do nothing
        }

        static ProcessService Instance;

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        public IProcessService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new ProcessService();
            }
            return Instance;
        }
    }
    #endregion
    #region 实现类
    /// <summary>
    /// 流程管理类
    /// </summary>
    internal class ProcessService : IProcessService
    {
        
        internal ProcessService()
        {
        }
        #region 流程控制-媒体预定

        #region 获取流程展示所需数据集合
        /// <summary>
        /// 获取当前流程所需展示的数据集
        /// </summary>
        /// <param name="ProcessId">流程编号</param>
        /// <param name="AdOrderId">订单编号编号</param>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="UserSys">用户信息</param>
        /// <param name="NodeName">节点名称</param>
        /// <returns>ProcessListModel</returns>
        public ProcessListModel GetProcessList(string ProcessId, string AdOrderId, string ProcessName, tblUser_Sys UserSys, string NodeName)
        {
            ProcessListModel pModel = new ProcessListModel();
            try
            {
                #region 流程管理-媒体预定
                ///整体思路，获取所有节点，并获取当前用户所要操作的节点，无则表示没有操作权限。
                ///有则判断是否已操作，已操作则不可编辑。
                pModel.ProcessId = ProcessId;
                //获取当前节点执行人。
                pModel.Sender = UserSys.UserName;
                pModel.ProcessName = ProcessName;

                //获取流程中所有节点名称
                pModel.NodeNames = ProcessDataHelp.listnodes(ProcessName);


                ////获取流程已执行信息,如无则可启动流程。
                //pModel.tblProcessList = db.tblProcessList.Where(a => a.ProcessId == AdOrderId && a.ProcessName == ProcessName).OrderByDescending(a => a.ExecuteDate).ToList();


                #region 判断是否有已执行的流程
                if (NodeName != "启动流程")
                {
                    //判断用户所需执行的节点，无信息则不能操作流程节点     
                    var Nodes = BusinessContext.tblProcessList.GetModelList("Accepter='" + pModel.Sender + "' and PassFlag=0 and ProcessId='" + ProcessId + "' and NodeName='" + NodeName + "'").Where(p => p.Accepter == pModel.Sender && p.PassFlag == false && p.ProcessId == ProcessId && p.NodeName == NodeName).Distinct().ToList();

                    tblProcessList tblTemp = new tblProcessList();
                    if (Nodes != null && Nodes.Count > 0)
                    {
                        tblTemp = Nodes[0];
                        pModel.TblRcdId = tblTemp.TblRcdID;
                        pModel.NodeName = tblTemp.NodeName;
                        #region 获取下个节点所需的数据
                        //获取下个节点的下拉数据,如果没有则表示已到最后节点
                        pModel.NextNodeName = GetNextRoelUserList(tblTemp.NodeName, ProcessName, ProcessId, AdOrderId);

                        //获取下个节点是否有结束，判断还需添加下个节点的流程
                        var IsOverTemp = BusinessContext.tblProcessManage.GetModelList("NodeName='" + NodeName + "'").Where(p => p.NodeName == NodeName).Select(a => a.NodeNameNext).Distinct();
                        if (IsOverTemp == null || IsOverTemp.Count() <= 0)
                            pModel.IsOver = true;

                        #endregion
                    }
                }
                #endregion

                #region 在无已执行流程的情况下，判断当前用户是否有权限启动流程
                else
                {
                    pModel.NodeName = pModel.NodeNames[0];
                    #region 获取下个节点所需的数据
                    //获取下个节点的下拉数据
                    pModel.NextNodeName = GetNextRoelUserList(pModel.NodeNames[0], ProcessName, ProcessId, AdOrderId);
                }
                    #endregion
            }
                #endregion

                #endregion
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("流程管理GetProcessList出错：", ex);
            }

            return pModel;
        }

        /// <summary>
        /// 获取有效日期（存在疑问）
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="validDays">有效天数</param>
        /// <returns></returns>
        public static DateTime GetValidDate(DateTime startDate, int validDays)
        {
            DateTime endDate = startDate.AddDays(validDays);
            List<DateTime> holiday = BusinessContext.tblStatutory_Set.GetModelList("1=1 ").Select(p => p.StatutorDays).ToList();
            for (; startDate <= endDate; startDate = startDate.AddDays(1))
            {
                if (holiday.Contains(startDate))
                {
                    endDate = endDate.AddDays(1);
                }
            }
            return endDate;
        }
        /// <summary>
        /// 根据节点名称，获取下个节点的接收节点下拉数据
        /// </summary>
        /// <param name="NodeName">流程节点</param>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="ProcessId">流程编号</param>
        /// <returns>eg:媒体预定-客户经理-zq</returns>
        public List<string> GetNextRoelUserList(string NodeName, string ProcessName, string ProcessId, string AdOrderId)
        {
            List<string> listnode = new List<string>();
            try
            {
                //获取流程节点状态，判断是否为结束节点
                var vIsOver = BusinessContext.tblProcessManage.GetModelList("NodeName='" + NodeName + "' and ProcessName='" + ProcessName + "'").Where(p => p.NodeName == NodeName).Select(s => s.NodeType).Distinct();

                ///根据流程节点名称获取下个节点名称
                var NextNodeTemp = BusinessContext.tblProcessManage.GetModelList("NodeName='" + NodeName + "' and ProcessName='" + ProcessName + "'").Where(p => p.NodeName == NodeName && p.ProcessName == ProcessName).Select(s => s.NodeNameNext).Distinct();
                if (ProcessName == "合同设计" && NodeName == "设计申请")
                {
                    List<tblAdOrder> AdOrders = BusinessContext.tblAdOrder.GetModelList("adorderid='" + AdOrderId + "'");
                    if (AdOrders != null && AdOrders.Count > 0)
                    {
                        tblAdOrder AdOrder = AdOrders[0];
                        bool ContractSureFlag = AdOrder.ContractSureFlag;
                        if (ContractSureFlag)
                        {
                            //合同状态走媒体审核
                            NextNodeTemp = NextNodeTemp.Where(p => !p.Contains("经理审批"));
                        }
                        else
                        {
                            //预订单状态要经理审核
                            NextNodeTemp = NextNodeTemp.Where(p => !p.Contains("媒体审核"));
                        }
                    }
                }
                if (vIsOver.Count() > 0 && vIsOver.ToList()[0].Contains("结束"))
                {
                    if (NextNodeTemp.Count() > 0)
                    {
                        foreach (var v in NextNodeTemp.ToList())
                        {
                            listnode.Add(v);
                        }
                    }
                    return listnode;
                }
                foreach (var vNodeName in NextNodeTemp)
                {
                    //判断详细表里是否已包含有对应的节点数据
                    //ReturnMessageModel RM = JudgeDetailData(ProcessName, vNodeName,ProcessId, AdOrderId);
                    //if (!RM.IsSuccess)
                    //    continue;

                    //根据流程节点获取角色
                    var NextNodeRole = BusinessContext.tblProcessManage.GetModelList("NodeName='" + vNodeName + "' and ProcessName='" + ProcessName + "'").Where(p => p.NodeName == vNodeName).Select(s => s.ExecutorRole).Distinct();
                    //根据角色获取用户并进行拼写
                    foreach (var vNextRole in NextNodeRole)
                    {
                        //获取角色id
                        var vRoleId = BusinessContext.sys_role.GetModelList("role_name='" + vNextRole + "'").Where(p => p.role_name == vNextRole).Select(s => s.TblRcdId).Distinct();
                        if (vRoleId.Count() > 0)
                        {
                            int iRoleId = vRoleId.ToList()[0];
                            //根据角色id获取角色所包含的用户登录名
                            var vLoginName = BusinessContext.tblUser_Roles.GetModelList("Role_Id='" + iRoleId + "'").Where(p => p.Role_Id == iRoleId).Select(s => s.LoginName).Distinct();
                            //根据用户登录名获取用户名
                            foreach (var vName in vLoginName)
                            {
                                var vUserName = BusinessContext.tblUser_Sys.GetModelList("LoginName='" + vName + "'").Where(p => p.LoginName == vName).Select(s => s.UserName).Distinct();
                                if (vUserName.Count() > 0)
                                    listnode.Add(string.Format("{0}-{1}-{2}", vNodeName, vNextRole, vUserName.ToList()[0]));
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("流程管理GetNextRoelUserList出错：", ex);
            }
            return listnode;
        }
        /// <summary>
        /// 判断节点是否包含有详细表数据
        /// </summary>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="NodeName">节点名称</param>
        /// <param name="AdOrderId">订单号</param>
        /// <returns>是or否</returns>
        public ReturnMessageModel JudgeDetailData(string ProcessName, string NodeName,string ProcessId, string AdOrderId)
        {
            bool bIsSuccess = true;
            ReturnMessageModel RM = new ReturnMessageModel();
            string SMessage = "";
            try
            {
                #region 判断详细表里是否已包含有对应的节点数据
                #region 媒体预定、订单确认、特殊执行申请
                ///判断详细表里是否已包含有数据
                if (ProcessName == "媒体预定" || ProcessName == "合同确认")
                {
                    int nFlag = 0;
                    var v1 = BusinessContext.tblOutdoorAdOrderMedia.GetModelList("AdOrderID='" + AdOrderId + "'").Where(p => p.AdOrderId == AdOrderId);
                    if (v1.Count() > 0)
                    {
                        nFlag++;
                        SMessage = ProcessDataHelp.MediaSure(AdOrderId, "tblOutdoorAdOrderMedia", NodeName, ProcessName);
                        if (SMessage == "MeidaOk")
                        {
                            bIsSuccess = true;
                        }
                        else
                        {
                            bIsSuccess = false;
                            RM.Message = RM.Message + SMessage;
                        }
                    }
                    var v2 = BusinessContext.tblBicycleMadialist.GetModelList("AdOrderID='" + AdOrderId + "'").Where(p => p.AdOrderID == AdOrderId);
                    if (v2.Count() > 0)
                    {
                        nFlag++;
                   
                        SMessage = ProcessDataHelp.MediaSure(AdOrderId, "tblBicycleMadialist", NodeName, ProcessName);
                        if (SMessage == "MeidaOk")
                        {
                            bIsSuccess = true;
                        }
                        else
                        {
                            bIsSuccess = false;
                            RM.Message = RM.Message + SMessage;
                        }
                    }
                    bool bFlag = false;
                    var v3 = BusinessContext.tblAdOrderMedialist.GetModelList("AdOrderID='" + AdOrderId + "'").Where(p => p.AdOrderId == AdOrderId);
                    if (v3.Count() > 0)
                    {
                        nFlag++;
                        bFlag = true;
                        SMessage = ProcessDataHelp.MediaSure(AdOrderId, "tblAdOrderMedialist", NodeName, ProcessName);
                        if (SMessage == "MeidaOk")
                        {
                            bIsSuccess = true;
                        }
                        else
                        {
                            bIsSuccess = false;
                            RM.Message = RM.Message + SMessage;
                        }
                    }
                    if (bFlag && ProcessName == "合同确认")
                    {
                        var v4 = BusinessContext.tblAdFixingList.GetModelList("AdOrderID='" + AdOrderId + "'").Where(p => p.AdOrderId == AdOrderId);
                        if (v4.Count() > 0)
                        {
                            nFlag++;
                            string Smessage = ProcessDataHelp.AdFixingListCheck(AdOrderId);
                            if (!string.IsNullOrWhiteSpace(Smessage))
                            {
                                bIsSuccess = false;
                                RM.Message += Smessage;
                            }
                        }
                        else
                        {
                            bIsSuccess = false;
                            RM.Message ="没有车位信息。请添加!";
                        }
                    }
                    if (nFlag == 0)
                    {
                        bIsSuccess = false;
                        RM.Message = AdOrderId+"没有明细。";
                    }
                    RM.IsSuccess = bIsSuccess;
                }
                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                RM.IsSuccess = false;
                RM.Message = ex.Message;
                Bitshare.PTMM.Common.LogManager.Error("流程管理JudgeDetailData出错：", ex);
            }
            return RM;
        }
        #endregion

        #region 获取流程页面节点控制显示的数据
        /// <summary>
        /// 获取流程在页面展示时所需的条件字段
        /// </summary>
        /// <param name="ProcessId">流程编号</param>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="UserSys">用户信息</param>
        /// <returns>ProcessListModel</returns>
        public ProcessListModel GetProcessShowData(string ProcessId, string ProcessName, string AdOrderId, tblUser_Sys UserSys)
        {
            ProcessListModel pModel = new ProcessListModel();
            ///用来获取错误信息，用以页面显示
            StringBuilder strbError = new StringBuilder();
            //获取是否为可操作用户
            pModel.IsUser = ProcessDataHelp.GetProcessIsUser(ProcessName, UserSys.LoginName);
            #region 流程管理-媒体预定
            ///整体思路，获取所有节点，并获取当前用户所要操作的节点，无则表示没有操作权限。
            ///有则判断是否已操作，已操作则不可编辑。
            #region 当用户包含在流程中时，判断用户处于流程的节点
            if (pModel.IsUser)
            {
                //获取流程已执行信息,如无则可启动流程。
                pModel.tblProcessList = BusinessContext.tblProcessList.GetModelList("ProcessId='" + ProcessId + "' and ProcessName='" + ProcessName + "'").OrderByDescending(a => a.ExecuteDate).ToList();

                List<string> list = new List<string>();
                #region 判断是否有已执行的流程
                if (pModel.tblProcessList.Count > 0)
                {
                    #region 判断是否添加重新启动按钮

                    //判断是否拥有重新启动的权限。1、为启动角色 2、媒体预定启动后，且无用户确认。
                    if (ProcessName == "媒体预定")
                    {
                        //判断当前流程所执行的节点数，已执行的超过一条则表示流程已启动且有确定节点  
                        var NodesAgain = BusinessContext.tblProcessList.GetModelList("").Where(p => p.PassFlag && p.ProcessId == ProcessId && p.ProcessName == ProcessName).Select(s => s.NodeName).Distinct().ToList();
                        if (NodesAgain.Count == 1)
                        {
                            //无执行流程的情况下，判断当前用户是否为可启动流程的用户
                            //首先获取第一个节点所需的角色
                            List<string> listRole = BusinessContext.tblProcessManage.GetModelList("ProcessName='" + ProcessName + "' and NodeNum=1").Where(p => p.ProcessName == ProcessName && p.NodeNum == 1).Select(a => a.ExecutorRole).Distinct().ToList();

                            //获取当前用的角色ID
                            List<int> listUserRole = BusinessContext.tblUser_Roles.GetModelList("LoginName='" + UserSys.LoginName + "'").Where(p => p.LoginName == UserSys.LoginName).Select(a => a.Role_Id).ToList();
                            ///根据2个数据集判断当前用户所担任的角色，是否包含在启动流程的角色中。
                            for (int i = 0; i < listRole.Count; i++)
                            {
                                string strRoleName = listRole[i];
                                var vTemp = BusinessContext.sys_role.GetModelList("role_name='" + strRoleName + "'").Where(p => p.role_name == strRoleName).Select(a => a.TblRcdId);
                                for (int j = 0; j < listUserRole.Count; j++)
                                {
                                    if (vTemp.Count() > 0 && vTemp.ToList()[0] == listUserRole[j])
                                    {
                                        //获取第一个节点名称---可提取到循环外部
                                        List<string> FirstNode = BusinessContext.tblProcessManage.GetModelList("ProcessName='" + ProcessName + "' and NodeNum=1").Where(p => p.ProcessName == ProcessName && p.NodeNum == 1).Select(a => a.NodeName).Distinct().ToList();
                                        if (FirstNode.Count > 0)
                                        {
                                            //判断下个节点是否有符合规则的数据，没有则不显示按钮
                                            if (GetNextRoelUserList(FirstNode.ToList()[0], ProcessName, ProcessId, AdOrderId).Count <= 0)
                                            {
                                                continue;
                                            }
                                            list.Add("重新启动");
                                            break;
                                        }
                                    }
                                }
                                if (pModel.IsStartProcess)
                                    break;
                            }
                        }
                    }

                    #endregion


                    //判断用户所需执行的节点名称，无信息则不能操作流程节点     
                    var Nodes = BusinessContext.tblProcessList.GetModelList("Accepter='" + UserSys.UserName + "' and PassFlag=0 and ProcessId='" + ProcessId + "' and ProcessName='" + ProcessName + "'").Select(s => s.NodeName).Distinct().ToList();
                    if (Nodes.Count() > 0)
                    {
                        ///判断所需执行的节点，上个节点是否已经执行完毕
                        foreach (var node in Nodes.ToList())
                        {
                            //判断下个节点是否有符合规则的数据，没有则不显示按钮
                            //if (GetNextRoelUserList(node, ProcessName, ProcessId, AdOrderId).Count <= 0)
                            //{
                            //    continue;
                            //}
                            //判断节点是否包含有详细表数据，无数据则不允许操作
                            //ReturnMessageModel RM=JudgeDetailData(ProcessName, node, ProcessId,AdOrderId);
                            //if (!RM.IsSuccess)
                            //{
                            //    strbError.AppendFormat(RM.Message);
                            //    continue;
                            //}
                            bool bTemp = false;
                            //获取当前节点的上个节点名称集合
                            var vTemp = BusinessContext.tblProcessManage.GetModelList("NodeNameNext='" + node + "'").Where(p => p.NodeNameNext == node).Select(s => s.NodeName);
                            //遍历上个节点是否有未完成的节点
                            foreach (var v1 in vTemp.ToList())
                            {
                                var v1Temp = BusinessContext.tblProcessList.GetModelList("NodeName='" + v1 + "' and PassFlag=1 and ProcessName ='" + ProcessName + "' and ProcessId='" + ProcessId + "'").Where(p => p.NodeName == v1 && !p.PassFlag && p.ProcessName == ProcessName && p.ProcessId == ProcessId);
                                if (v1Temp.Count() > 0)
                                {
                                    strbError.AppendFormat("上节点尚未确认完全,{0} 暂不可操作", node);
                                    bTemp = true; break;
                                }
                            }
                            if (!bTemp)
                                list.Add(node);

                        }
                        //获取用户所需执行的节点名称
                        pModel.UserNodeNames = list;

                        #region 判断当前流程是否已结束


                        ////获取流程设定中为结束的节点名称
                        //var LastNode = db.tblProcessManage.Where(p => p.NodeType == "结束"&&p.ProcessName==ProcessName).Select(s => s.NodeName).Distinct();
                        //foreach (var v in LastNode.ToList())
                        //{
                        //    //判断节点状态为结束的节点，是否已经执行。已执行则表示流程结束
                        //    var IsOverTemp = db.tblProcessList.Where(p => p.NodeName == v && !p.PassFlag && p.ProcessId == AdOrderId).Select(a => a.NodeName).Distinct();
                        //    if (IsOverTemp.Count() <= 0)
                        //    {
                        //        pModel.IsOver = true;
                        //        break;
                        //    }
                        //}
                        #endregion
                    }
                }
                #endregion
                #region 在无已执行流程的情况下，判断当前用户是否有权限启动流程
                else
                {
                    //无执行流程的情况下，判断当前用户是否为可启动流程的用户
                    //首先获取第一个节点所需的角色
                    List<string> listRole = BusinessContext.tblProcessManage.GetModelList("ProcessName='" + ProcessName + "' and NodeNum=1 ").Where(p => p.ProcessName == ProcessName && p.NodeNum == 1).Select(a => a.ExecutorRole).Distinct().ToList();

                    //获取当前用的角色ID
                    List<int> listUserRole = BusinessContext.tblUser_Roles.GetModelList("LoginName='" + UserSys.LoginName + "'").Where(p => p.LoginName == UserSys.LoginName).Select(a => a.Role_Id).ToList();
                    ///根据2个数据集判断当前用户所担任的角色，是否包含在启动流程的角色中。
                    for (int i = 0; i < listRole.Count; i++)
                    {
                        string strRoleName = listRole[i];
                        var vTemp = BusinessContext.sys_role.GetModelList("role_name='" + strRoleName + "'").Where(p => p.role_name == strRoleName).Select(a => a.TblRcdId);
                        for (int j = 0; j < listUserRole.Count; j++)
                        {
                            if (vTemp.Count() > 0 && vTemp.ToList()[0] == listUserRole[j])
                            {
                                ///获取第一个节点名称
                                List<string> FirstNode = BusinessContext.tblProcessManage.GetModelList("ProcessName='" + ProcessName + "' and NodeNum=1").Where(p => p.ProcessName == ProcessName && p.NodeNum == 1).Select(a => a.NodeName).Distinct().ToList();
                                if (FirstNode.Count > 0)
                                {
                                    //判断下个节点是否有符合规则的数据，没有则不显示按钮
                                    if (GetNextRoelUserList(FirstNode.ToList()[0], ProcessName, ProcessId, AdOrderId).Count <= 0)
                                    {
                                        strbError.Append("有未符合规则的数据");
                                        continue;
                                    }
                                    pModel.IsStartProcess = true;
                                    list.Add("启动流程");
                                    pModel.UserNodeNames = list;
                                    break;
                                }
                            }
                        }
                        if (pModel.IsStartProcess)
                            break;
                    }

                }
                #endregion
            }
            #endregion
            #endregion
            //返回显示时所需的提示信息，使用Opinion字段替用
            pModel.Opinion = strbError.ToString();
            return pModel;
        }


        #endregion

        #region 保存数据前判断操作

        /// <summary>
        /// 在执行保存前，对数据进行判断操作。
        /// </summary>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="NodeName">节点名称</param>
        /// <param name="ProcessId">流程id</param>
        /// <param name="AdOrderId">订单id</param>
        /// <returns></returns>
        public ReturnMessageModel JudgeProcessData(string ProcessName, string NodeName, string ProcessId, string AdOrderId)
        {
            ReturnMessageModel RM = new ReturnMessageModel(true);
            //判断当前节点是否包含有详情表信息
            if (ProcessName == "媒体预定" || ProcessName == "合同确认")
            {
                RM=JudgeDetailData(ProcessName, NodeName,ProcessId,AdOrderId);
                if (!RM.IsSuccess)
                {
                    return RM;
                }
            }
            if (ProcessName == "合同设计")
            {
               return RM=AdDesignData(ProcessName, NodeName, ProcessId);
            }
            if (ProcessName == "媒体制作合同" || ProcessName == "自备车制作合同" || ProcessName == "代理合同")
            {
                //自备车制作 _tblAdFixinglist_Zi
                //媒体制作 _tblOutdoorAdOrderMedia_Zi  _tblAdFixinglist_Zi
                //代理合同  三个
                var v3 = BusinessContext.tblAdFixingList_Zi.GetModelList("AdOrderID='" + AdOrderId + "'").Where(p => p.AdOrderId == AdOrderId);
                if (v3.Count() > 0)
                {
                    RM.IsSuccess = true;
                }
                else if (ProcessName != "自备车制作合同")
                {
                    var v2 = BusinessContext.tblOutdoorAdOrderMedia_Zi.GetModelList("AdOrderID='" + AdOrderId + "'").Where(p => p.AdOrderID == AdOrderId);
                    if (v2.Count() > 0)
                    {
                        RM.IsSuccess = true;
                    }
                    else if (ProcessName != "媒体制作合同")
                    {
                        var v1 = BusinessContext.tblAdOrderMedialist_Zi.GetModelList("AdOrderID='" + AdOrderId + "'").Where(p => p.AdOrderId == AdOrderId);
                        if (v1.Count() > 0)
                        {
                            RM.IsSuccess = true;
                        }
                        else
                        {
                            RM.IsSuccess = false;
                            RM.Message = "没有媒体明细。";
                        }
                    }
                    else
                    {
                        RM.IsSuccess = false;
                        RM.Message = "没有户外明细。";
                    }
                }
                else
                {
                    RM.IsSuccess = false;
                    RM.Message = "没有车位明细。";
                }
            }
            if (ProcessName == "车身广告监测")
            {
                if (NodeName=="监测提交"||NodeName=="经理审核监测"||NodeName=="监测确认")
                {
                    var v1 = BusinessContext.tblAdFixingList_Monitor.GetModelList("ProcessId='" + ProcessId + "'");
                    if (v1.Count() > 0)
                    {
                        RM.IsSuccess = true;
                    }
                    else
                    {
                        RM.IsSuccess = false;
                        RM.Message = "没有车位明细。";
                    }
                }
            }
            if (ProcessName == "户外广告监测")
            {
                if (NodeName == "监测提交" || NodeName == "经理审核监测" || NodeName == "监测确认")
                {
                    var v1 = BusinessContext.tblOutdoorAdOrderMedia_Monitor.GetModelList("ProcessId='" + ProcessId + "'");
                    if (v1.Count() > 0)
                    {
                        RM.IsSuccess = true;
                    }
                    else
                    {
                        RM.IsSuccess = false;
                        RM.Message = "没有户外明细。";
                    }
                }
            }
            if (ProcessName == "自行车广告监测")
            {
                if (NodeName == "监测提交" || NodeName == "经理审核监测" || NodeName == "监测确认")
                {
                    var v1 = BusinessContext.tblBicycleMadialist_Monitor.GetModelList("ProcessId='" + ProcessId + "'");
                    if (v1.Count() > 0)
                    {
                        RM.IsSuccess = true;
                    }
                    else
                    {
                        RM.IsSuccess = false;
                        RM.Message = "没有自行车明细。";
                    }
                }
            }
            if (ProcessName.Contains("广告制作"))
            {
                //制作申请	1	经理确认	1必须有户外明细		
                //经理确认	2	媒体审核	1必须有户外明细		
                //媒体审核	3	喷绘下单	1必须有户外明细		
                //喷绘下单	4	喷绘完成	1必须有户外明细;2必须填写喷绘下单时间		
                //喷绘完成	5	制作下单	1必须有户外明细;2.必须填写喷绘交付时间		
                //制作下单	6	制作安排	1必须有户外明细;2.判断是否有空位。		
                //制作安排	7	制作完成	1必须有户外明细;2.判断是否有空位		
                //制作完成	8	结束	1必须有户外明细;2.判断是否有空位；3安装日期必填		
                RM = ProcessDataHelp.CheckMakingByFun(ProcessId, ProcessName, NodeName);
                if (RM.IsSuccess)
                {
                    if (NodeName == "喷绘下单")
                    {
                        #region 喷绘下单时间
                        if (ProcessName == "车身广告制作")
                        {
                            string strSql = " select tblrcdid from tblAdFixingList_Making where Processid='" + ProcessId + "' and (DrawsDate is null or DrawsDate='')";
                            DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                RM.IsSuccess = false;
                                RM.Message = "当前制作单有广告信息喷绘下单时间为空";
                            }
                            else
                            {
                                RM.IsSuccess = true;
                            }
                        }
                        if (ProcessName == "户外广告制作")
                        {
                            string strSql = " select tblrcdid from tblOutdoorAdOrderMedia_Making where Processid='" + ProcessId + "' and (DrawsDate is null or DrawsDate='')";
                            DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                RM.IsSuccess = false;
                                RM.Message = "当前制作单有广告信息喷绘下单时间为空";
                            }
                            else
                            {
                                RM.IsSuccess = true;
                            }
                        }
                        if (ProcessName == "自行车广告制作")
                        {
                            string strSql = " select tblrcdid from tblBicycleMadialist_Making where Processid='" + ProcessId + "' and (DrawsDate is null or DrawsDate='')";
                            DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                RM.IsSuccess = false;
                                RM.Message = "当前制作单有广告信息喷绘下单时间为空";
                            }
                            else
                            {
                                RM.IsSuccess = true;
                            }
                        }
                        if (ProcessName == "其他广告制作")
                        {
                            bool bl = false;
                            string strSql = " select tblrcdid from tblAdFixingList_Making where Processid='" + ProcessId + "' and (DrawsDate is null or DrawsDate='')";
                            DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                bl = true;
                            }
                            else
                            {
                                strSql = " select tblrcdid from tblOutdoorAdOrderMedia_Making where Processid='" + ProcessId + "' and (DrawsDate is null or DrawsDate='')";
                                dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    bl = true;
                                }
                                else
                                {
                                    strSql = " select tblrcdid from tblBicycleMadialist_Making where Processid='" + ProcessId + "' and (DrawsDate is null or DrawsDate='')";
                                    dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        bl = true;
                                    }
                                }
                            }
                            if (bl)
                            {
                                RM.IsSuccess = false;
                                RM.Message = "当前制作单有广告信息喷绘下单时间为空";
                            }
                            else
                            {
                                RM.IsSuccess = true;
                            }
                        }
                        #endregion
                    }
                    else if (NodeName == "喷绘完成")
                    {
                        #region 喷绘完成时间
                        if (ProcessName == "车身广告制作")
                        {
                            string strSql = " select tblrcdid from tblAdFixingList_Making where Processid='" + ProcessId + "' and (OverDate is null or OverDate='')";
                            DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                RM.IsSuccess = false;
                                RM.Message = "当前制作单有广告信息喷绘完成时间为空";
                            }
                            else
                            {
                                RM.IsSuccess = true;
                            }
                        }
                        if (ProcessName == "户外广告制作")
                        {
                            string strSql = " select tblrcdid from tblOutdoorAdOrderMedia_Making where Processid='" + ProcessId + "' and (OverDate is null or OverDate='')";
                            DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                RM.IsSuccess = false;
                                RM.Message = "当前制作单有广告信息喷绘完成时间为空";
                            }
                            else
                            {
                                RM.IsSuccess = true;
                            }
                        }
                        if (ProcessName == "自行车广告制作")
                        {
                            string strSql = " select tblrcdid from tblBicycleMadialist_Making where Processid='" + ProcessId + "' and (OverDate is null or OverDate='')";
                            DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                RM.IsSuccess = false;
                                RM.Message = "当前制作单有广告信息喷绘完成时间为空";
                            }
                            else
                            {
                                RM.IsSuccess = true;
                            }
                        }
                        if (ProcessName == "其他广告制作")
                        {
                            bool bl = false;
                            string strSql = " select tblrcdid from tblAdFixingList_Making where Processid='" + ProcessId + "' and (OverDate is null or OverDate='')";
                            DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                bl = true;
                            }
                            else
                            {
                                strSql = " select tblrcdid from tblOutdoorAdOrderMedia_Making where Processid='" + ProcessId + "' and (OverDate is null or OverDate='')";
                                dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    bl = true;
                                }
                                else
                                {
                                    strSql = " select tblrcdid from tblBicycleMadialist_Making where Processid='" + ProcessId + "' and (OverDate is null or OverDate='')";
                                    dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        bl = true;
                                    }
                                }
                            }
                            if (bl)
                            {
                                RM.IsSuccess = false;
                                RM.Message = "当前制作单有广告信息喷绘完成时间为空";
                            }
                            else
                            {
                                RM.IsSuccess = true;
                            }
                        }
                        #endregion
                    }
                    else if (NodeName == "制作下单" || NodeName == "制作安排" || NodeName == "制作完成")
                    {
                        bool IsSuccess = true;
                        string sMessage = "";
                        if (NodeName == "制作下单")
                        {
                            #region 制作下单时间
                            if (ProcessName == "车身广告制作")
                            {
                                string strSql = " select tblrcdid from tblAdFixingList_Making where Processid='" + ProcessId + "' and (MakeorderDate is null or MakeorderDate='')";
                                DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    RM.IsSuccess = false;
                                    IsSuccess = false;
                                    sMessage += "当前制作单有广告信息制作下单时间为空，";
                                    RM.Message = sMessage;
                                }
                                else
                                {
                                    RM.IsSuccess = true;
                                }
                            }
                            if (ProcessName == "户外广告制作")
                            {
                                string strSql = " select tblrcdid from tblOutdoorAdOrderMedia_Making where Processid='" + ProcessId + "' and (MakeorderDate is null or MakeorderDate='')";
                                DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    RM.IsSuccess = false;
                                    IsSuccess = false;
                                    sMessage+= "当前制作单有广告信息制作下单时间为空，";
                                    RM.Message = sMessage;
                                }
                                else
                                {
                                    RM.IsSuccess = true;
                                }
                            }
                            if (ProcessName == "自行车广告制作")
                            {
                                string strSql = " select tblrcdid from tblBicycleMadialist_Making where Processid='" + ProcessId + "' and (MakeorderDate is null or MakeorderDate='')";
                                DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    RM.IsSuccess = false;
                                    IsSuccess = false;
                                    sMessage += "当前制作单有广告信息制作下单时间为空，";
                                    RM.Message = sMessage;
                                }
                                else
                                {
                                    RM.IsSuccess = true;
                                }
                            }
                            #endregion
                        }
                        if (NodeName == "制作完成")
                        {
                            #region 制作完成时间
                            if (ProcessName == "车身广告制作")
                            {
                                string strSql = " select tblrcdid from tblAdFixingList_Making where Processid='" + ProcessId + "' and (MakeorderEndDate is null or MakeorderEndDate='')";
                                DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    RM.IsSuccess = false;
                                    IsSuccess = false;
                                    sMessage += "当前制作单有广告信息制作完成时间为空，";
                                    RM.Message = sMessage;
                                }
                                else
                                {
                                    RM.IsSuccess = true;
                                }
                            }
                            if (ProcessName == "户外广告制作")
                            {
                                string strSql = " select tblrcdid from tblOutdoorAdOrderMedia_Making where Processid='" + ProcessId + "' and (MakeorderEndDate is null or MakeorderEndDate='')";
                                DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    RM.IsSuccess = false;
                                    IsSuccess = false;
                                    sMessage += "当前制作单有广告信息制完成单时间为空，";
                                    RM.Message = sMessage;
                                }
                                else
                                {
                                    RM.IsSuccess = true;
                                }
                            }
                            if (ProcessName == "自行车广告制作")
                            {
                                string strSql = " select tblrcdid from tblBicycleMadialist_Making where Processid='" + ProcessId + "' and (MakeorderEndDate is null or MakeorderEndDate='')";
                                DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    RM.IsSuccess = false;
                                    IsSuccess = false;
                                    sMessage += "当前制作单有广告信息制作完成时间为空，";
                                    RM.Message = sMessage;
                                }
                                else
                                {
                                    RM.IsSuccess = true;
                                }
                            }
                            if (ProcessName == "其他广告制作")
                            {
                                bool bl = false;
                                string strSql = " select tblrcdid from tblAdFixingList_Making where Processid='" + ProcessId + "' and (MakeorderEndDate is null or MakeorderEndDate='')";
                                DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    bl = true;
                                }
                                else
                                {
                                    strSql = " select tblrcdid from tblOutdoorAdOrderMedia_Making where Processid='" + ProcessId + "' and (MakeorderEndDate is null or MakeorderEndDate='')";
                                    dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        bl = true;
                                    }
                                    else
                                    {
                                        strSql = " select tblrcdid from tblBicycleMadialist_Making where Processid='" + ProcessId + "' and (MakeorderEndDate is null or MakeorderEndDate='')";
                                        dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                        if (dt != null && dt.Rows.Count > 0)
                                        {
                                            bl = true;
                                        }
                                    }
                                }
                                if (bl)
                                {
                                    RM.IsSuccess = false;
                                    RM.Message = "当前制作单有广告信息喷绘完成时间为空";
                                }
                                else
                                {
                                    RM.IsSuccess = true;
                                }
                            }
                            #endregion
                        }
                        #region 判断空位
                        if (RM.IsSuccess || NodeName == "制作完成")
                        {
                            //判断空位
                            if (ProcessName == "车身广告制作")
                            {
                                //判断空位
                                List<tblAdFixingList_Making> listFixing = BusinessContext.tblAdFixingList_Making.GetModelList("ProcessId='" + ProcessId + "'");
                                RM = ProcessDataHelp.VehicleMediaCheckByMake(ProcessId, AdOrderId, listFixing);
                                if (RM.IsSuccess && NodeName == "制作完成")
                                {
                                    string strSql = " select tblrcdid from tblAdFixingList_Making where Processid='" + ProcessId + "' and (FixDate is null or FixDate='')";
                                    DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        RM.IsSuccess = false;
                                        sMessage += "当前制作单有广告信息安装日期为空";
                                        RM.Message = sMessage;
                                    }
                                    else
                                    {
                                        RM.IsSuccess = true;
                                    }
                                    RM.IsSuccess = IsSuccess;
                                    RM.Message = sMessage;
                                }
                            }
                            //判断空位
                            else if (ProcessName == "户外广告制作")
                            {
                                //判断空位
                                List<tblOutdoorAdOrderMedia_Making> listFixing = BusinessContext.tblOutdoorAdOrderMedia_Making.GetModelList("ProcessId='" + ProcessId + "'");
                                RM = ProcessDataHelp.OutMediaCheck(AdOrderId, listFixing);
                                if (RM.IsSuccess && NodeName == "制作完成")
                                {
                                    string strSql = " select tblrcdid from tblOutdoorAdOrderMedia_Making where Processid='" + ProcessId + "' and (FixDate is null or FixDate='')";
                                    DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        RM.IsSuccess = false;
                                        sMessage += "当前制作单有广告信息安装日期为空";
                                        RM.Message = sMessage;
                                    }
                                    else
                                    {
                                        RM.IsSuccess = true;
                                    }
                                    RM.IsSuccess = IsSuccess;
                                    RM.Message = sMessage;
                                }
                            }
                            else if (ProcessName == "自行车广告制作")
                            {
                                //判断空位
                                List<tblBicycleMadialist_Making> listFixing = BusinessContext.tblBicycleMadialist_Making.GetModelList("ProcessId='" + ProcessId + "'");
                                RM = ProcessDataHelp.BicycleMediaCheck(AdOrderId, listFixing);
                                if (RM.IsSuccess && NodeName == "制作完成")
                                {
                                    string strSql = " select tblrcdid from tblBicycleMadialist_Making where Processid='" + ProcessId + "' and (FixDate is null or FixDate='')";
                                    DataTable dt = DBContext.PTMMHZ.GetDataTable(strSql);
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        RM.IsSuccess = false;
                                        sMessage += "当前制作单有广告信息安装日期为空";
                                        RM.Message = sMessage;
                                    }
                                    else
                                    {
                                        RM.IsSuccess = true;
                                    }
                                    RM.IsSuccess = IsSuccess;
                                    RM.Message = sMessage;
                                }
                            }
                        #endregion
                        }
                    }
                }

            }
            return RM;
        }

        public ReturnMessageModel AdDesignData(string ProcessName, string NodeName, string ProcessId)
        {
            ReturnMessageModel RM = new ReturnMessageModel(true);
            DataTable AdDesignDs = DBContext.PTMMHZ.GetDataTable("select SubInfo,Designer,Evaluation,AdOrderId from tblAdDesign  where ProcessId='" + ProcessId + "'");
            string SubInfo = "";
            string Designer = "";
            string Evaluation = "";
            string AdOrderId = "";
            if (AdDesignDs != null && AdDesignDs.Rows.Count > 0)
            {
                SubInfo=Convert.ToString(AdDesignDs.Rows[0]["SubInfo"]);
                Designer = Convert.ToString(AdDesignDs.Rows[0]["Designer"]);
                Evaluation = Convert.ToString(AdDesignDs.Rows[0]["Evaluation"]);
                AdOrderId = Convert.ToString(AdDesignDs.Rows[0]["AdOrderId"]);
            }
            //1，判断是否有设计明细。2提交资料
            if (NodeName == "设计申请")
            {
                int nFlag = 0;
                var v1 = BusinessContext.tblAdOrderMediaList_Design.GetModelList("ProcessId='" + ProcessId + "'").Where(p => p.ProcessId == ProcessId);
                if (v1.Count() >0)
                    nFlag++;

                var v2 = BusinessContext.tblAdFixinglist_Design.GetModelList("ProcessId='" + ProcessId + "'").Where(p => p.ProcessId == ProcessId);
                if (v2.Count() >0)
                    nFlag++;

                var v4 = BusinessContext.tblOutdoorAdOrderMedia_Design.GetModelList("ProcessId='" + ProcessId + "'").Where(p => p.ProcessID == ProcessId);
                if (v4.Count() > 0)
                    nFlag++;

                var v3 = BusinessContext.tblBicycleMadialist_Design.GetModelList("ProcessId='" + ProcessId + "'").Where(p => p.ProcessID == ProcessId);
                if (v3.Count() > 0)
                    nFlag++;
                if (nFlag==0)
                {
                        RM.IsSuccess = false;
                        RM.Message = string.Format("{0} 节点不包含媒体信息。", NodeName);
                    return RM;
                }
                if (string.IsNullOrEmpty(SubInfo))
	            {
		            RM.IsSuccess = false;
                     RM.Message = string.Format("{0} 节点请填写提交资料。", NodeName);
                    return RM;
	            }
                
               
            }
            //选择设计师
            if (NodeName == "设计派单")
            {
                if (string.IsNullOrEmpty(Designer))
                {
                    RM.IsSuccess = false;
                    RM.Message = string.Format("{0} 节点请选择设计师。", NodeName);
                    return RM;
                }
            }
            //填写评价
            if (NodeName == "确认定稿")
            {
                if (string.IsNullOrEmpty(Evaluation))
                {
                    RM.IsSuccess = false;
                    RM.Message = string.Format("{0} 节点请填写评价。", NodeName);
                    return RM;
                }
                #region 流程走完后更新设计人、小样色号和设计类型，胶贴到达时间，交接时间
                //更新设计人、小样色号和设计类型，胶贴到达时间，交接时间
                List<string> SqlList = new List<string>();
                string strCs = "select SmallColor,DesignType,AdContent,MediaName,RoadSortName,VehicleTypeName,MeidaNum,InsureIssueDate,InsureEndDate from tblAdOrderMediaList_Design where ProcessId='" + ProcessId + "'";     //车身媒体
                string strVehicle = "select SmallColor,DesignType,StickersDate,SetDate,AdContent,MediaName,RoadSortName,VehicleTypeName,VehicleNumbering,InsureIssueDate,InsureEndDate from tblAdFixinglist_Design where ProcessId='" + ProcessId + "'";
                string strOut = "select SmallColor,DesignType,StickersDate,SetDate,AdContent,MediaName,RoadName,PtPlatForm,Azimuth,OutDoorMediaNumbering,IssueDate,endDate from tblOutdoorAdOrderMedia_Design where ProcessId='" + ProcessId + "'";
                string strBicycle = "select SmallColor,DesignType,StickersDate,SetDate,AdContent,MediaName,RoadName,ServiceName,StopId,IssueDate,endDate from tblBicycleMadialist_Design where ProcessId='" + ProcessId + "'";
                DataTable dtCs = DBContext.PTMMHZ.GetDataTable(strCs);
                DataTable dtVehicle = DBContext.PTMMHZ.GetDataTable(strVehicle);
                DataTable dtOut = DBContext.PTMMHZ.GetDataTable(strOut);
                DataTable dtBicycle = DBContext.PTMMHZ.GetDataTable(strBicycle);
                string SmallColor="";
                string DesignType = "";
                string StickersDate = "";
                string SetDate = "";
                
                string strList = "";


                for (int i = 0; i < dtCs.Rows.Count; i++)
                {
                    string AdContent = "";//广告品牌
                    string MediaName = "";//媒体名称
                    string RoadSortName = "";//线路分类
                    string VehicleTypeName = "";//车型名称
                    string MeidaNum = "";//数量
                    string InsureIssueDate = "";//发布日期
                    string InsureEndDate = "";//结束日期
                    if (dtCs != null && dtCs.Rows.Count > 0)
                    {

                        SmallColor = Convert.ToString(dtCs.Rows[i]["SmallColor"]);
                        DesignType = Convert.ToString(dtCs.Rows[i]["DesignType"]);
                        AdContent = Convert.ToString(dtCs.Rows[i]["AdContent"]);
                        MediaName = Convert.ToString(dtCs.Rows[i]["MediaName"]);
                        RoadSortName = Convert.ToString(dtCs.Rows[i]["RoadSortName"]);
                        VehicleTypeName = Convert.ToString(dtCs.Rows[i]["VehicleTypeName"]);
                        MeidaNum = Convert.ToString(dtCs.Rows[i]["MeidaNum"]);
                        InsureIssueDate = Convert.ToString(dtCs.Rows[i]["InsureIssueDate"]);
                        InsureEndDate = Convert.ToString(dtCs.Rows[i]["InsureEndDate"]);
                        

                    }
                    strList = "update tblAdOrderMediaList set Designer='" + Designer + "',SmallColor ='" + SmallColor + "', DesignType='" + DesignType + "'  where AdOrderId='" + AdOrderId + "' and AdContent= '" + AdContent + "' and MediaName= '" + MediaName + "' and RoadSortName= '" + RoadSortName + "' and VehicleTypeName= '" + VehicleTypeName + "' and MeidaNum= '" + MeidaNum + "' and InsureIssueDate= '" + InsureIssueDate + "' and InsureEndDate= '" + InsureEndDate + "'";
                    SqlList.Add(strList);
                }

                for (int i = 0; i < dtVehicle.Rows.Count; i++)
                {
                    string AdContent = "";//广告品牌
                    string MediaName = "";//媒体名称
                    string RoadSortName = "";//线路分类
                    string VehicleTypeName = "";//车型名称
                    string VehicleNumbering = "";//车辆编号
                    string InsureIssueDate = "";//发布日期
                    string InsureEndDate = "";//结束日期
                    if (dtVehicle != null && dtVehicle.Rows.Count > 0)
                    {
                        
                        SmallColor = Convert.ToString(dtVehicle.Rows[i]["SmallColor"]);
                        DesignType = Convert.ToString(dtVehicle.Rows[i]["DesignType"]);
                        StickersDate = Convert.ToString(dtVehicle.Rows[i]["StickersDate"]);
                        SetDate = Convert.ToString(dtVehicle.Rows[i]["SetDate"]);
                        AdContent = Convert.ToString(dtVehicle.Rows[i]["AdContent"]);
                        MediaName = Convert.ToString(dtVehicle.Rows[i]["MediaName"]);
                        RoadSortName = Convert.ToString(dtVehicle.Rows[i]["RoadSortName"]);
                        VehicleTypeName = Convert.ToString(dtVehicle.Rows[i]["VehicleTypeName"]);
                        VehicleNumbering = Convert.ToString(dtVehicle.Rows[i]["VehicleNumbering"]);
                        InsureIssueDate = Convert.ToString(dtVehicle.Rows[i]["InsureIssueDate"]);
                        InsureEndDate = Convert.ToString(dtVehicle.Rows[i]["InsureEndDate"]);
                        

                    }
                    strList = "update tblAdFixinglist set Designer='" + Designer + "',SmallColor ='" + SmallColor + "', DesignType='" + DesignType + "', StickersDate='" + StickersDate + "', SetDate='" + SetDate + "'  where AdOrderId='" + AdOrderId + "' and AdContent= '" + AdContent + "' and MediaName= '" + MediaName + "' and RoadSortName= '" + RoadSortName + "' and VehicleTypeName= '" + VehicleTypeName + "' and VehicleNumbering= '" + VehicleNumbering + "' and InsureIssueDate= '" + InsureIssueDate + "' and InsureEndDate= '" + InsureEndDate + "'";
                    SqlList.Add(strList);
                }
                for (int i = 0; i < dtOut.Rows.Count; i++)
                {
                    string AdContent = "";//广告品牌
                    string MediaName = "";//媒体名称
                    string RoadName = "";//路段名
                    string PtPlatForm = "";//站台名
                    string Azimuth = "";//方位
                    string OutDoorMediaNumbering = "";//编号
                    string InsureIssueDate = "";//发布日期
                    string InsureEndDate = "";//结束日期
                    if (dtOut != null && dtOut.Rows.Count > 0)
                    {
                        SmallColor = Convert.ToString(dtOut.Rows[i]["SmallColor"]);
                        DesignType = Convert.ToString(dtOut.Rows[i]["DesignType"]);
                        StickersDate = Convert.ToString(dtOut.Rows[i]["StickersDate"]);
                        SetDate = Convert.ToString(dtOut.Rows[i]["SetDate"]);
                        AdContent = Convert.ToString(dtOut.Rows[i]["AdContent"]);
                        MediaName = Convert.ToString(dtOut.Rows[i]["MediaName"]);
                        RoadName = Convert.ToString(dtOut.Rows[i]["RoadName"]);
                        PtPlatForm = Convert.ToString(dtOut.Rows[i]["PtPlatForm"]);
                        Azimuth = Convert.ToString(dtOut.Rows[i]["Azimuth"]);
                        OutDoorMediaNumbering = Convert.ToString(dtOut.Rows[i]["OutDoorMediaNumbering"]);
                        InsureIssueDate = Convert.ToString(dtOut.Rows[i]["IssueDate"]);
                        InsureEndDate = Convert.ToString(dtOut.Rows[i]["EndDate"]);
                    }
                    strList = "update tblOutdoorAdOrderMedia set Designer='" + Designer + "',SmallColor ='" + SmallColor + "', DesignType='" + DesignType + "', StickersDate='" + StickersDate + "', SetDate='" + SetDate + "'  where AdOrderId='" + AdOrderId + "' and AdContent= '" + AdContent + "' and OutDoorMediaName= '" + MediaName + "' and RoadName= '" + RoadName + "' and PtPlatForm= '" + PtPlatForm + "' and Azimuth= '" + Azimuth + "' and OutDoorMediaNumbering= '" + OutDoorMediaNumbering + "'  and IssueDate= '" + InsureIssueDate + "' and EndDate= '" + InsureEndDate + "'";
                    SqlList.Add(strList);
                }

                for (int i = 0; i < dtBicycle.Rows.Count; i++)
                {
                    string AdContent = "";//广告品牌
                    string MediaName = "";//媒体名称
                    string RoadName = "";//路段名
                    string ServiceName = "";//服务点名称
                    string StopId = "";//服务点编号
                    string InsureIssueDate = "";//发布日期
                    string InsureEndDate = "";//结束日期
                    if (dtBicycle != null && dtBicycle.Rows.Count > 0)
                    {
                        SmallColor = Convert.ToString(dtBicycle.Rows[i]["SmallColor"]);
                        DesignType = Convert.ToString(dtBicycle.Rows[i]["DesignType"]);
                        StickersDate = Convert.ToString(dtBicycle.Rows[i]["StickersDate"]);
                        SetDate = Convert.ToString(dtBicycle.Rows[i]["SetDate"]);
                        AdContent = Convert.ToString(dtBicycle.Rows[i]["AdContent"]);
                        MediaName = Convert.ToString(dtBicycle.Rows[i]["MediaName"]);
                        RoadName = Convert.ToString(dtBicycle.Rows[i]["RoadName"]);
                        ServiceName = Convert.ToString(dtBicycle.Rows[i]["ServiceName"]);
                        StopId = Convert.ToString(dtBicycle.Rows[i]["StopId"]);
                        InsureIssueDate = Convert.ToString(dtBicycle.Rows[i]["IssueDate"]);
                        InsureEndDate = Convert.ToString(dtBicycle.Rows[i]["EndDate"]);
                    }
                    strList = "update tblBicycleMadialist set Designer='" + Designer + "',SmallColor ='" + SmallColor + "', DesignType='" + DesignType + "', StickersDate='" + StickersDate + "', SetDate='" + SetDate + "'  where AdOrderId='" + AdOrderId + "' and AdContent= '" + AdContent + "' and OutDoorMediaName= '" + MediaName + "' and RoadName= '" + RoadName + "' and ServiceName= '" + ServiceName + "' and StopId= '" + StopId + "'  and IssueDate= '" + InsureIssueDate + "' and EndDate= '" + InsureEndDate + "'";
                    SqlList.Add(strList);
                }
                
                bool IsSuccess = DBContext.PTMMHZ.ExecTrans(SqlList);
                #endregion


            }
            return RM;
        }



        #endregion

        #region 撤销流程操作
        /// <summary>
        /// 撤销流程操作
        /// </summary>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="NodeName">节点名称</param>
        /// <param name="ProcessId">流程编号</param>
        /// <param name="AdOrderId">合同号</param>
        /// <param name="UserName">用户类</param>
        /// <param name="Opinion">意见</param>
        /// <returns></returns>
        public Hashtable RevocationProcess(string ProcessName, string NodeName, string ProcessId, string AdOrderId, tblUser_Sys UserName, string Opinion)
        {
            Hashtable result = new Hashtable();
            List<string> sqllist = new List<string>();
            Boolean isbool = false;
            string msg = "";
            try
            {
                string strSql = "";
                string NodeType = "";
                strSql = "select NodeType from tblProcessManage where ProcessName='" + ProcessName + "' and NodeName='" + NodeName + "'";
                DataTable sread = DBContext.PTMMHZ.GetDataTable(strSql);
                if (sread.Rows.Count > 0)
                {
                    NodeType = Convert.ToString(sread.Rows[0]["NodeType"]);
                }               
                if (NodeType == "开始")
                {
                    msg = "当前流程状态不可撤销！";
                    isbool = false;
                }
                else
                {
                    if (ProcessName.Contains("订料"))
                    {
                        strSql = "select AdorderId from tblAdMaterial where ProcessId='" + ProcessId + "'";
                        SqlDataReader reader = DBContext.PTMMHZ.GetReader(strSql);
                        if (reader.Read())
                        {
                            AdOrderId = Convert.ToString(reader["AdorderId"]);
                        }
                        reader.Close();
                    }
                    if (ProcessName.Contains("设计申请"))
                    {
                        strSql = "select AdorderId from tblAdDesign where ProcessId='" + ProcessId + "'";
                        SqlDataReader reader = DBContext.PTMMHZ.GetReader(strSql);
                        if (reader.Read())
                        {
                            AdOrderId = Convert.ToString(reader["AdorderId"]);
                        }
                        reader.Close();
                    }
                    if (ProcessName.Contains("监测") || ProcessName.Contains("刊布"))
                    {
                        strSql = "select AdorderId from tblAdMonitor where ProcessId='" + ProcessId + "'";
                        SqlDataReader reader = DBContext.PTMMHZ.GetReader(strSql);
                        if (reader.Read())
                        {
                            AdOrderId = Convert.ToString(reader["AdorderId"]);
                        }
                        reader.Close();
                    }
                    strSql = "select tblRcdid from dbo.tblProcessList where PassFlag=0 and  ProcessName='" + ProcessName + "' and ProcessId='" + ProcessId + "'";
                    DataTable PassFlagds = DBContext.PTMMHZ.GetDataTable(strSql);
                    if (PassFlagds.Rows.Count == 0)
                    {
                        msg = "当前流程已经结束或尚未开启，无法撤销！";
                        isbool = false;
                    }
                    else
                    {
                        strSql = "select distinct Accepter  from VIEW_Process where ProcessName='" + ProcessName + "' and ProcessId='" + ProcessId + "' and PassFlag=1";
                        DataTable ds = DBContext.PTMMHZ.GetDataTable(strSql);
                        if (ds.Rows.Count > 0)
                        {
                            string Accepter = "";
                            for (int i = 0; i < ds.Rows.Count; i++)
                            {
                                Accepter = Convert.ToString(ds.Rows[i]["Accepter"]);
                                string AdOrderInfo = ",订单号：" + AdOrderId;
                                string MsgContent = "流程号：" + ProcessId + AdOrderInfo + ",流程名称：" + ProcessName + "," + NodeName + "已被" + UserName.UserName + "撤销,撤销原因：" + Opinion;
                                strSql = "insert INTO tblMessage (Sender,Accepter,MsgType,MsgTitle,MsgContent,SendDate,AcceptDate,State,ProcessId,AdOrderId) values('系统消息','" + Accepter + "','" + "撤销通知" + "','" + ProcessName + "撤销通知" + "','" + MsgContent + "','" + DateTime.Now + "','" + DateTime.Now + "',0,'" + ProcessId + "','" + AdOrderId + "')";
                                sqllist.Add(strSql);
                            }
                            Accepter = Convert.ToString(ds.Rows[0]["Accepter"]);
                            if (ProcessName == "媒体预定")
                            {
                                strSql = "update  tblOutdoorMediaList set MediaState=0 where AdOrderId='" + AdOrderId + "'";
                                sqllist.Add(strSql);

                                strSql = "update  tblParkingMetreMediaList set MediaState=0 where AdOrderId='" + AdOrderId + "'";
                                sqllist.Add(strSql);

                                strSql = "update  tblLSAirportMediaList set MediaState=0 where AdOrderId='" + AdOrderId + "'";
                                sqllist.Add(strSql);

                                strSql = "update  tblBuildMediaList set MediaState=0 where AdOrderId='" + AdOrderId + "'";
                                sqllist.Add(strSql);

                                strSql = "update  tblStationMediaList set MediaState=0 where AdOrderId='" + AdOrderId + "'";
                                sqllist.Add(strSql);

                                strSql = "update  tblTaxiMediaList set MediaState=0 where AdOrderId='" + AdOrderId + "'";
                                sqllist.Add(strSql);

                                strSql = "update  tblMagaMediaList set MediaState=0 where AdOrderId='" + AdOrderId + "'";
                                sqllist.Add(strSql);
                            }
                            if (ProcessName == "订单确认")
                            {
                                strSql = "update  tblOutdoorMediaList set MediaState=2 where AdOrderId='" + AdOrderId + "'";
                                sqllist.Add(strSql);

                                strSql = "update  tblParkingMetreMediaList set MediaState=2 where AdOrderId='" + AdOrderId + "'";
                                sqllist.Add(strSql);

                                strSql = "update  tblLSAirportMediaList set MediaState=2 where AdOrderId='" + AdOrderId + "'";
                                sqllist.Add(strSql);

                                strSql = "update  tblBuildMediaList set MediaState=2 where AdOrderId='" + AdOrderId + "'";
                                sqllist.Add(strSql);

                                strSql = "update  tblStationMediaList set MediaState=2 where AdOrderId='" + AdOrderId + "'";
                                sqllist.Add(strSql);

                                strSql = "update  tblTaxiMediaList set MediaState=2 where AdOrderId='" + AdOrderId + "'";
                                sqllist.Add(strSql);

                                strSql = "update  tblMagaMediaList set MediaState=2 where AdOrderId='" + AdOrderId + "'";
                                sqllist.Add(strSql);
                            }
                            if (ProcessName == "开票登记")
                            {
                                strSql = "delete  tblAdInvoice where AdOrderId='" + ProcessId + "'";
                                sqllist.Add(strSql);
                            }
                            strSql = "delete tblProcessList where   ProcessId='" + ProcessId + "' and ProcessName='" + ProcessName + "'";
                            sqllist.Add(strSql);

                            strSql = "insert INTO tblProcessList_Revocation (ProcessId, AdOrderId, ProcessName, NodeName, Sender, SendDate, Accepter,AcceptLoginName, Opinion) values('" + ProcessId + "','" + AdOrderId + "','" + ProcessName + "','" + NodeName + "','" + Accepter + "',getdate(),'" + UserName.UserName + "','" + UserName.LoginName + "','" + Opinion + "')";
                            sqllist.Add(strSql);
                            if (DBContext.PTMMHZ.ExecTrans(sqllist.ToArray()))
                            {
                                isbool = true;
                                msg = "撤销成功！";
                            }
                            else
                            {
                                msg = "撤销失败！";
                                isbool = false;
                            }
                        }
                        else
                        {
                            msg = "当前流程状态不可撤销！";
                            isbool = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Add("result", false);
                result.Add("msg", "失败" + ex.Message);
            }
            finally
            {
            }
            result.Add("result", isbool);
            result.Add("msg", msg);
            return result;
        }
        #endregion   

        #endregion
        #region 获取页面信息
        /// <summary>
        ///拼装分页参数对象
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="SWhere">查询条件</param>
        /// <returns></returns>
        private PageInfo GetPageInfo(string loginName,int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "",
            string SWhere = " 1=1 ")
        {

            PageInfo page = new PageInfo();
            page.CurrentPageIndex = pageIndex;
            page.PageSize = pageSize;
            page.QueryFields = queryFields;
            if (String.IsNullOrWhiteSpace(orderBy))
            {
                orderBy = " TblRcdId Desc";
            }
            page.Orderby = orderBy;
            page.Where = SWhere;
            return page;
        }
        /// <summary>
        /// 分页获取合同详细
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="SWhere">查询条件</param>
        /// <returns>返回对象</returns>
        public SubPageResult<tblProcessList> GetProcessListPageResult(string loginName,  int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "",string SWhere = null)
        {
            SubPageResult<tblProcessList> result = new SubPageResult<tblProcessList>();
            try
            {
                PageInfo page = GetPageInfo(loginName,  pageIndex, pageSize, queryFields, orderBy, SWhere);
                result = CommonHelper.ListSubPageResult<tblProcessList>(page);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetProcessListPageResult", ex);
            }
            return result;
        }
        /// <summary>
        /// 分页获取合同详细
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="SWhere">查询条件</param>
        /// <returns>返回对象</returns>
        public SubPageResult<tblProcesslist_Temp> GetOldProcessListPageData(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string SWhere = null)
        {
            SubPageResult<tblProcesslist_Temp> result = new SubPageResult<tblProcesslist_Temp>();
            try
            {
                PageInfo page = GetPageInfo(loginName, pageIndex, pageSize, queryFields, orderBy, SWhere);
                result = CommonHelper.ListSubPageResult<tblProcesslist_Temp>(page);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetOldProcessListPageData", ex);
            }
            return result;
        }
        #endregion
        #region 保存数据操作
        /// <summary>
        /// 获取下一节点信息(节点名和接收人)
        /// </summary>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="NodeName">节点名称</param>
        /// <returns>返回内容(Hashtable["NodeName"],Hashtable["Accepter"])</returns>
        public static List<Hashtable> GetNodeNextInfo(string[] NextNodeName)
        {
            List<Hashtable> results = new List<Hashtable>();
            try
            {
                Hashtable result = null;
                string[] arr = null;
                //string[] NextNodes = NextNodeName.Split(',');
                foreach (string NextNode in NextNodeName)
                {
                    if (string.IsNullOrWhiteSpace(NextNode))
                    {
                        continue;
                    } 
                    result = new Hashtable();
                    arr = NextNode.Split('-'); // 节点名-角色-用户
                    result.Add("NodeName", arr[0]);
                    if (arr.Length > 2)
                    {
                        result.Add("Accepter", arr[2]);
                    }
                    results.Add(result);
                }
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetNodeNextInfo", ex);
            }
            return results;
        }
        /// <summary>
        /// 获取当前节点类型信息(开始/正常/结束)
        /// </summary>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="NodeName">节点名称</param>
        /// <returns></returns>
        public static string GetNodeInfo(string ProcessName, string NodeName, string ProcessId, string[] NextNodeNames)
        {
            string NodeType = string.Empty;
            try
            {
                List<Hashtable> NextNodes = GetNodeNextInfo(NextNodeNames);
                List<string> nextNodeNameList = new List<string>();
                foreach (Hashtable NextNode in NextNodes)
                {
                    string NextNodeName = (string)NextNode["NodeName"];
                    if (!nextNodeNameList.Contains(NextNodeName))
                    {
                        nextNodeNameList.Add(NextNodeName);
                    }
                }

                NodeType = BusinessContext.tblProcessManage.GetModelList("ProcessName='" + ProcessName + "' and NodeName='" + NodeName + "'").Where(p => p.ProcessName == ProcessName && p.NodeName == NodeName && nextNodeNameList.Contains(p.NodeNameNext)).Select(p => p.NodeType).Distinct().FirstOrDefault();
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetNodeInfo", ex);
            }
            return NodeType;
        }
        /// <summary>
        /// 执行当前节点数据库更新 及 下级节点节点信息的插入
        /// </summary>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="NodeName">节点名称</param>
        /// <param name="ProcessId">流程编号</param>
        /// <param name="AdOrderId">流程对应定单号</param>
        /// <param name="Sender">当前用户ID</param>
        /// <param name="Opinion">当前节点 处理意见</param>
        /// <param name="NextNodeName">下级节点，字符串连接用，隔开</param>
        /// <param name="NodeType">节点类型</param>
        /// <param name="_sqlList">流程相关的SQL语句</param>
        /// <returns></returns>
        public static List<string> SaveProcessInfo(string ProcessName, string NodeName, string ProcessId, string AdOrderId, string Opinion, string[] NextNodeNames, tblUser_Sys US)
        {
            List<string> sqllist = new List<string>();
         
            string msg = string.Empty;
            string NodeType = GetNodeInfo(ProcessName, NodeName, ProcessId, NextNodeNames);
            try
            {
                if (NodeType == "开始")
                {
                    #region 执行的操作
                    //插入开始流程节点
                    string strSql = "insert into tblProcessList (ProcessId,AdOrderId,ProcessName,NodeName,Sender,Opinion,PassFlag,Executor,ExecuteDate,Accepter)" +
                              "values('" + ProcessId + "','" + AdOrderId + "','" + ProcessName + "','" + NodeName + "','" + US.UserName + "','" + Opinion + "',1,'" + US.UserName + "','" + DateTime.Now + "','" + US.UserName + "')";
                    sqllist.Add(strSql);

                    // 插入流程下个需要执行的节点
                    List<Hashtable> NextNodes = GetNodeNextInfo(NextNodeNames);
                    foreach (Hashtable NextNode in NextNodes)
                    {
                        string NextNodeName = (string)NextNode["NodeName"];
                        string Accepter = (string)NextNode["Accepter"];
                        strSql = "insert into tblProcessList (ProcessId,AdOrderId,ProcessName,NodeName,Sender,Accepter,Opinion)" +
                             "values('" + ProcessId + "','" + AdOrderId + "','" + ProcessName + "','" + NextNodeName + "','" + US.UserName + "','" + Accepter + "','')";
                        sqllist.Add(strSql);
                    }
                    #endregion
                }
                else if (NodeType == "正常")
                {
                    #region 执行的操作
                    //更新当点节点工作为已完成
                    string strSql = "update tblProcessList set PassFlag=1,Executor='" + US.UserName + "',ExecuteDate='" + DateTime.Now + "',Opinion='" + Opinion + "'  where ProcessName='" + ProcessName + "' and nodename='" + NodeName + "' and ProcessId='" + ProcessId + "' and Accepter='" + US.UserName + "'";
                    sqllist.Add(strSql);
                    // 插入流程下个需要执行的节点
                    List<Hashtable> NextNodes = GetNodeNextInfo(NextNodeNames);
                    foreach (Hashtable NextNode in NextNodes)
                    {
                        string NextNodeName = (string)NextNode["NodeName"];
                        string Accepter = (string)NextNode["Accepter"];
                        strSql = "insert into tblProcessList (ProcessId,AdOrderId,ProcessName,NodeName,Sender,Accepter,Opinion)" +
                             "values('" + ProcessId + "','" + AdOrderId + "','" + ProcessName + "','" + NextNodeName + "','" + US.UserName + "','" + Accepter + "','')";
                        sqllist.Add(strSql);
                    }


                    #endregion
                }
                else if (NodeType == "结束")
                {
                    string strSql = "";
                    //更新当点节点工作为已完成
                    strSql = "update tblProcessList set PassFlag=1,Executor='" + US.UserName + "',ExecuteDate='" + DateTime.Now + "',Opinion='" + Opinion + "'  where ProcessName='" + ProcessName + "' and nodename='" + NodeName + "' and ProcessId='" + ProcessId + "' and Accepter='" + US.UserName + "'";
                    sqllist.Add(strSql);
                    if (ProcessName == "媒体制作合同" || ProcessName == "自备车制作合同" || ProcessName == "代理合同")
                    {
                        //插入媒体预定流程
                        string sqls = "insert into tblProcessList (ProcessId,AdOrderId,ProcessName,NodeName,Sender,Opinion,PassFlag,Executor,ExecuteDate,Accepter)" +
                                  "values('" + ProcessId + "','" + AdOrderId + "','" + ProcessName + "','" + NodeName + "','" + US.UserName + "','" + Opinion + "',1,'" + US.UserName + "','" + DateTime.Now.Date + "','" + US.UserName + "')";
                        sqllist.Add(sqls);
                    }
                }
                //List<string> lMess = MessageSend(ProcessId, ProcessName, NodeName, AdOrderId);
                //sqllist.AddRange(lMess);
            }
            catch (Exception ex)
            {
              
                msg = ex.Message;
                Bitshare.PTMM.Common.LogManager.Error("SaveNodeInfo", ex);
            }
            return sqllist;
        }
        /// <summary>
        /// 保存节点数据，并根据节点对相关表状态进行更新
        /// </summary>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="NodeName">节点名称</param>
        /// <param name="ProcessId">流程编号</param>
        /// <param name="AdOrderId">订单编号</param>
        /// <param name="NextNodeName"></param>
        /// <param name="Opinion"></param>
        /// <param name="US"></param>
        /// <returns></returns>
        public ReturnMessageModel SaveNodeInfo(string ProcessName, string NodeName, string ProcessId, string AdOrderId, string Opinion, string[] NextNodeName, tblUser_Sys US, string UserName, string LoginName)
        {
            bool result = true;
            ReturnMessageModel RM = new ReturnMessageModel();
            string strSql = "";
            try
            {
                List<string> sqlList = new List<string>();
                #region 媒体预定
                if (ProcessName == "媒体预定" && NodeName.Contains("媒体确认"))
                {
                    strSql = "update tblOutdoorAdOrderMedia set MediaState = 2 ,ContractSureDate='" + DateTime.Now.ToShortDateString() + "', ContractFlag=1 where AdOrderId='" + AdOrderId + "'";
                    sqlList.Add(strSql);
                    strSql = "update tblBicycleMadialist set MediaState = 2 ,ContractSureDate='" + DateTime.Now.ToShortDateString() + "', ContractFlag=1 where AdOrderId='" + AdOrderId + "'";
                    sqlList.Add(strSql);
                    //媒体预订明细
                    strSql = "select *from tblAdOrderMedialist where  AdOrderId='" + AdOrderId + "'";
                    DataTable ds_AdOrderMedia = DBContext.PTMMHZ.GetDataTable(strSql);
                    if (ds_AdOrderMedia != null && ds_AdOrderMedia.Rows.Count > 0)
                    {
                        //由车身生成车位
                        List<string> ListSql = ProcessDataHelp.GetSqlListByNodeName(AdOrderId);
                        if (ListSql != null && ListSql.Count > 0)
                        {
                            sqlList.AddRange(ListSql);
                        }
                        else
                        {
                            result = RM.IsSuccess = false;
                            RM.Message = "分配失败，请检查媒体空位情况。";
                        }
                        //本地是4个工作日,外地（客户）10工作日自动保存
                        DateTime StartDate = new DateTime();
                        StartDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        DateTime EffectDate = GetValidDate(StartDate.AddDays(-1), 4);
                      
                        DataTable Dt = DBContext.PTMMHZ.GetDataTable("select LocalOrRemote from tblAdOrder where AdOrderId='" + AdOrderId + "'");
                        if (Dt != null && Dt.Rows.Count > 0)
                        {
                            string LocalOrRemote = Convert.ToString(Dt.Rows[0]["LocalOrRemote"]);
                            if (LocalOrRemote.Contains("外地"))
                            {
                                EffectDate = GetValidDate(StartDate.AddDays(-1), 10);
                            }
                            strSql = "update tblAdOrder set EffectDate='" + EffectDate + "',ContractDate='" + StartDate + "' where AdOrderId='" + AdOrderId + "'";
                        }
                        else
                        {
                            strSql = "update tblAdOrder set EffectDate='" + EffectDate + "',ContractDate='" + StartDate + "'  where AdOrderId='" + AdOrderId + "'";
                        }
                        sqlList.Add(strSql);
                    }
                    else 
                    {
                        //本地、外地（客户）均都是4个工作日
                        DateTime StartDate = new DateTime();
                        StartDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        DateTime EffectDate = GetValidDate(StartDate.AddDays(-1), 4);
                        strSql = "update tblAdOrder set EffectDate='" + EffectDate + "',ContractDate='" + StartDate + "'  where AdOrderId='" + AdOrderId + "'";
                        sqlList.Add(strSql);
                    }
//                    必须先有客户登记才能发起预定
//                    2、预订单如果是本地（客户），当销售点击发起时间后本地的4个工作日，自动保存，且不可修改；外地（客户）10工作日自动保存，且不可修改。（系统可自动判定时间是4还是10，不需要销售员点选）
//                      3、预订发起时告知销售经理

                    //GetValidDate(DateTime.Now,7);
                }
                #endregion
                #region 合同确认
                if (ProcessName == "合同确认")
                {
                    sqlList.AddRange(NewMethod(NodeName, AdOrderId));
                }
                #endregion
                #region 媒体制作，媒体代理，自备车制作
                if (ProcessName == "媒体制作合同" || ProcessName == "自备车制作合同" || ProcessName == "代理合同")
                {
                    strSql = "update tblAdOrder set ContractSureFlag = 1,ContractSureDate = '" + DateTime.Now.ToShortDateString() + "' where AdOrderId = '" + ProcessId + "'";
                    sqlList.Add(strSql);
                }
                #endregion
                #region  车身广告制作,户外广告制作,自行车广告制作
                if (ProcessName.Contains("广告制作") && NodeName == "制作完成")
                {
                    if (ProcessName == "自行车广告制作")
                    {
                        Dictionary<bool, List<String>> dGetOutMedia = Bitshare.PTMM.Service.Helper.ProcessDataHelp.BicycleMediaing(ProcessId, AdOrderId);
                        foreach (var item in dGetOutMedia)
                        {
                            if (item.Key)
                            {
                                sqlList.AddRange(item.Value);
                                RM.IsSuccess = true;
                            }
                            else
                            {
                                RM.IsSuccess = false;
                                RM.Message = CommonHelper.GetAllInfoByList(item.Value) + "在原始订单中不存在，或者发布日期和结束日期不同,或者安装日期没填写。";
                                return RM;
                            }
                        }
                    }
                    if (ProcessName == "户外广告制作")
                    {
                        Dictionary<bool, List<String>> dGetOutMedia = Bitshare.PTMM.Service.Helper.ProcessDataHelp.OutMediaing(ProcessId, AdOrderId);
                        foreach (var item in dGetOutMedia)
                        {
                            if (item.Key)
                            {
                                sqlList.AddRange(item.Value);
                                RM.IsSuccess = true;
                            }
                            else
                            {
                                RM.IsSuccess = false;
                                RM.Message = CommonHelper.GetAllInfoByList(item.Value) + "在原始订单中不存在，或者发布日期和结束日期不同,或者安装日期没填写。";
                                return RM;
                            }
                        }
                    }
                    if (ProcessName == "车身广告制作")
                    {
                        Dictionary<bool, List<String>> dGetOutMedia = Bitshare.PTMM.Service.Helper.ProcessDataHelp.VehicleMediaing(ProcessId, AdOrderId);
                        foreach (var item in dGetOutMedia)
                        {
                            if (item.Key)
                            {
                                sqlList.AddRange(item.Value);
                                RM.IsSuccess = true;
                            }
                            else
                            {
                                RM.IsSuccess = false;
                                RM.Message = CommonHelper.GetAllInfoByList(item.Value) + "在原始订单中不存在，或者发布日期和结束日期不同,或者安装日期没填写。";
                                return RM;
                            }
                        }
                    }
                }
                #endregion
                #region 流程保存
                sqlList.AddRange(SaveProcessInfo(ProcessName, NodeName, ProcessId, AdOrderId, Opinion, NextNodeName, US));
                #endregion
                if (result)
                {
                    if (sqlList.Count > 0)
                    {
                        result = DBContext.PTMMHZ.ExecTrans(sqlList.ToArray());
                        RM.IsSuccess = result;
                    }
                    foreach(string sql in sqlList)
                    {
                        OperateLogHelper.SaveOperateLog(LoginName, UserName, sql, "流程");
                    }
                }
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("流程管理SaveNodeInfo出错：", ex);
            }

            return RM;

        }

        private List<string> NewMethod(string NodeName, string AdOrderId)
        {
            

            List<string> sqlList=new List<string>();
            if (NodeName == "合同确认")
            {
                string strSql = "update tblAdOrder set ContractSureDate='" + DateTime.Now.ToShortDateString() + "', ContractSureFlag=1 where AdOrderId='" + AdOrderId + "'";
                sqlList.Add(strSql);
                strSql = "update tblOutdoorAdOrderMedia set MediaState=3 where AdOrderId='" + AdOrderId + "'";
                sqlList.Add(strSql);
                strSql = "update tblAdFixinglist set MediaState=3 where AdOrderId='" + AdOrderId + "'";
                sqlList.Add(strSql);
                strSql = "update tblBicycleMadialist set MediaState=3 where AdOrderId='" + AdOrderId + "'";
                sqlList.Add(strSql);
                #region 计算发布日期和到期日期并计算出合同期限
                string sql = "select adorderId,CONVERT(varchar(100),  min(issuedate), 23) issueDate,CONVERT(varchar(100),  max(enddate), 23) endDate,DATEDIFF(month, min(issuedate), max(enddate)) ContractPeriod from " +
                "(select adorderId,issueDate,endDate from tblOutdoorAdOrderMedia " +
                "union all " +
                "select adorderId,issueDate,endDate from tblBicycleMadialist " +
                "union all " +
                "select adorderId,InsureissueDate as issueDate,InsureendDate as endDate from tblAdFixinglist " +
                ") t " +
                "where adorderid='" + AdOrderId + "' group by adorderId;";
                DataTable dt = DBContext.PTMMHZ.GetDataTable(sql);
               
                if (dt != null && dt.Rows.Count > 0)
                {
                    DateTime StartDate;
                    DateTime EndDate;
                    string ContractPeriod = "";
                    StartDate = Convert.ToDateTime(dt.Rows[0]["issueDate"]);
                    EndDate = Convert.ToDateTime(dt.Rows[0]["endDate"]);
                    ContractPeriod = Convert.ToString(dt.Rows[0]["ContractPeriod"]) + "个月";

                    string AdClienteleSql = "select AdClientele from tblAdOrder where AdOrderId='" + AdOrderId + "'";
                    DataTable AdClienteleDt = DBContext.PTMMHZ.GetDataTable(AdClienteleSql);
                    string AdClientele = Convert.ToString(AdClienteleDt.Rows[0]["AdClientele"]);

                    strSql = "update tblAdOperationRegist set SignedState=1,OrderBelong='" + AdOrderId + "',RegisterDate='" + DateTime.Now.ToShortDateString() + "',StartDate='" + StartDate + "',EndDate='" + EndDate + "',ContractPeriod='" + ContractPeriod + "'  where Subscriber='" + AdClientele + "'";
                    sqlList.Add(strSql);
                }
                #endregion
        

            }
            return sqlList;
        }



        /// <summary>
        /// 根据订单和流程编号获取添加流程提示信息
        /// </summary>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="NodeName">节点名称</param>
        /// <param name="ProcessId">流程id</param>
        /// <param name="AdOrderId">订单id</param>
        /// <returns>ture/false</returns>
        public bool SaveMessage(string ProcessName, string NodeName, string ProcessId, string AdOrderId)
        {
            bool bIsSuccess = true;
            List<string> bSqlList = new List<string>();
            //获取该流程节点所配置的通知角色
            var vNoticeRole = BusinessContext.tblProcessNotice.GetModelList("ProcessName='" + ProcessName + "' and NodeName='" + NodeName + "'").Where(p => p.ProcessName == ProcessName && p.NodeName == NodeName).Select(s => s.NoticeRole);
            if (vNoticeRole.Count() > 0)
            {
                foreach (string notice in vNoticeRole.ToList())
                {
                    var vRoleId = BusinessContext.sys_role.GetModelList("role_name='" + notice + "'").Where(p => p.role_name == notice).Select(s => s.TblRcdId);
                    if (vRoleId.Count() > 0)
                    {
                        int iRoleId = vRoleId.ToList()[0];
                        List<string> listLonginName = new List<string>();
                        //如果为客户经理，只需通知发起流程的用户
                        if (notice == "客户经理")
                        {
                            var v = BusinessContext.tblProcessList.GetModelList("ProcessId='" + ProcessId + "' and ProcessName='" + ProcessName + "'").Where(p => p.ProcessId == ProcessId && p.ProcessName == ProcessName).OrderBy(o => o.TblRcdID).Select(s => s.Sender);//可以通过订单表获取订单表的业务员信息。
                            if (v.Count() > 0)
                            {
                                listLonginName.Add(v.ToList()[0]);
                            }
                        }
                        else
                        {
                            //获取角色id所对应的用户LoginName
                            var v = BusinessContext.tblUser_Roles.GetModelList("Role_Id=" + iRoleId).Where(p => p.Role_Id == iRoleId).Select(s => s.LoginName).Distinct();
                            if (v.Count() > 0)
                                listLonginName.AddRange(v.ToList());
                        }
                        foreach (string loginName in listLonginName)
                        {
                            string strUserName = string.Empty;
                            if (notice == "客户经理")
                            {
                                strUserName = loginName;
                            }
                            else
                            {
                                //根据loginName获取UserName
                                var vUserNameList = BusinessContext.tblUser_Sys.GetModelList("LoginName='" + loginName + "'").Where(p => p.LoginName == loginName).Select(s => s.UserName);
                                if (vUserNameList.Count() > 0)
                                {
                                    strUserName = vUserNameList.ToList()[0];
                                }
                            }
                            if (!string.IsNullOrEmpty(strUserName))
                            {
                                tblMessage message = new tblMessage();
                                message.Sender = "系统消息";
                                message.Accepter = strUserName;
                                message.MsgType = "通知";
                                message.MsgTitle = NodeName + "通知";
                                message.MsgContent = string.Format("流程号：{0}，订单号：{1}，{2}-{3}完成", ProcessId, AdOrderId, ProcessName, NodeName);
                                message.SendDate = DateTime.Now;
                                message.State = false;
                                message.ProcessId = ProcessId;
                                message.AdOrderId = AdOrderId;
                                //db.tblMessage.AddObject(message);
                                //db.SaveChanges();
                                string sql = string.Format("insert into tblMessage (Sender,Accepter,MsgType,MsgTitle,MsgContent,SendDate,State,ProcessId,AdOrderId) values('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}','{8}')", message.Sender, message.Accepter, message.MsgType, message.MsgTitle, message.MsgContent, message.SendDate, 0, ProcessId, AdOrderId);
                                bSqlList.Add(sql);
                            }
                        }
                    }
                }
                bIsSuccess=DBContext.PTMMHZ.ExecTrans(bSqlList);

            }
            return bIsSuccess;
        }

        #endregion
    }
#endregion
}
