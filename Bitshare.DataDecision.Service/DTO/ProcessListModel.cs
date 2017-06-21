using System;
using System.Collections.Generic;
using Bitshare.DataDecision.Model;
namespace Bitshare.DataDecision.Service.DTO
{
    /// <summary>
    /// 流程管理数据集合类
    /// </summary>
    public class ProcessListModel
    {
        /// <summary>
        /// 流程详细信息
        /// </summary>
        public List<tblProcessList> tblProcessList { get; set; }

        /// <summary>
        /// 当前流程节点名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 执行人
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 流程编号
        /// </summary>
        public string ProcessId { get; set; }

        /// <summary>
        /// 当前节点
        /// </summary>
        public string NodeName { get; set; }
       
        /// <summary>
        /// 下一节点
        /// </summary>
        public List<string> NextNodeName { get; set; }


        /// <summary>
        /// 下一节点可操作人
        /// </summary>
        public List<string> NextNodeSender { get; set; }

        /// <summary>
        /// 意见
        /// </summary>
        public string Opinion { get; set; }

        /// <summary>
        /// 当前用户操作的流程Rid
        /// </summary>
        public int Rid { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsOver { get; set; }

        /// <summary>
        /// 是否为可操作人
        /// </summary>
        public bool IsUser { get; set; }

        /// <summary>
        /// 是否未开启流程
        /// </summary>
        public bool IsStartProcess { get; set; }

        /// <summary>
        /// 流程所有的节点
        /// </summary>
        public List<string> NodeNames { get; set; }

        /// <summary>
        /// 用户所需执行的节点名称
        /// </summary>
        public List<string> UserNodeNames { get; set; }
    }
}
