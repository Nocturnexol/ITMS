/// <reference path="publicjs/jquery-1.4.1-vsdoc.js" />
//DOM 初始化完成之后会执行的内容
//用于小车回放的参数
var maplet = null; //地图对象
var marker = null; //标注对象
var menu = null;
var indexcity = "上海市";
var apiType = 1; //是否为明文经纬度
function Jquery_Ajax(jsondata, rqurl, sync) {
    var html = $.ajax({
        contentType: "application/json",
        url: rqurl,
        async: sync,
        type: 'post',
        data: jsondata,
        dataType: 'text',
        timeout: 1000000,
        error: function (ex) {
            alert('数据查询出错，或者远程连被中断！');
            return false;
        },
        success: function (json) {
        }
    }).responseText;
    return html;
}

//第二种方式适用于同步请求不需要返回值的情况 和异步请求
function Jquery_SyncAjax(jsondata, rqurl, sync, callback) {
    $.ajax({
        contentType: "application/json",
        url: rqurl,
        async: sync,
        type: 'post',
        data: jsondata,
        dataType: 'text',
        timeout: 1000000,
        beforeSend: function (XMLHttpRequest) {
            var strajaxload = "<div id='ajaxload' style='position:absolute;background-color: #000000;"
            strajaxload += "width:" + $(document).width() + "px;";
            strajaxload += "height:" + ($(document).height()) + "px;"
            strajaxload += "opacity:0.5;filter:alpha(opacity=0);"
            strajaxload += "top:0px;left:0px;z-index=999999'><center>正在加载数据请稍候...</center></div>";
            $(document.body).append(strajaxload);

        },
        complete: function (XMLHttpRequest, textStatus) {
            $("#ajaxload").remove();
        },
        error: function (ex) {
            alert('请求数据出错，请刷新页面后重试！');
            return false;
        },
        success: callback
    });
}


var chepic = null;
var lines = null
var counter = 0;
var timerId = null;
//
var whereList = new Array();
var points = new Array();
var globaldata;
var temppoint; // 全局变量用来保存原来的点的位置
$(document).ready(function () {

    $("#sltCdt").change(function () {
        $("#sltPathDirection").css("display", "none");
        if ($("#sltCdt").val() == "站名") {
            $("#sltPathDirection").css("display", "block");
        }
        var strCdt = $("#sltCdt").val();
        if (strCdt != "-查询条件-") {

            if (strCdt != "区域") {
                var jsondata = null;
                switch (strCdt) {
                    case "区属":
                        jsondata = '{table_name:"tblArea",str_field:"Area",str_where:""}';
                        break;
                    case "路名":
                        jsondata = '{table_name:"tblStop3",str_field:"RoadName",str_where:""}';
                        break;
                    case "站名":
                        jsondata = '{table_name:"tblStop3",str_field:"StationName",str_where:""}';
                        break;
                    default:
                        return false;
                        break;
                }
                var rqurl = '/WebService.asmx/GetFieldByWhere';
                var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
                var Jsonstr = eval('(' + callbackdata + ')');
                var item = Jsonstr.d;
                if (item == null) {
                    return false;
                }
                else {
                    whereList = item;
                }
            }
            else {
                whereList.push("内环");
                whereList.push("外环");
            }
        }
    });
    avBubble.width = 240;
    avBubble.height = 150;
    $("#slRoadLine_more").change(function () {
        var RoadLine = $(this).val();
        var lines = new Array();
        var RoadLines = $("#txtRoadLines").val().split(',');
        var OldRoadLine = "";
        var isnodirection = false;
        $(RoadLines).each(function (i) {
            var RoadInfos = RoadLines[i].split('-');
            if (RoadInfos.length > 1) {
                if (RoadInfos[1] == "") {
                    isnodirection = true;
                    OldRoadLine = RoadInfos[0];
                }
            }
            if (RoadLines[i] != "") {
                if ($.inArray(RoadLines[i], lines) == -1) {
                    lines.push(RoadLines[i]);
                }
            }
        });
        if (isnodirection) {
            if (RoadLine == OldRoadLine) {
            } else {
                alert("请选择线路的开往！");
                $(this).val( OldRoadLine);
            }
            return;
        }
        if ($.inArray(RoadLine, lines) == -1) {
            lines.push(RoadLine + "-");
            $("#slToDirection_more").empty();
            var options = "<option value='-开往-'>-开往-</option>";
            var str_where = "RoadLine='" + RoadLine + "'";
            var jsondata = '{table_name:"tblRoadBaseInfo",str_field:"ToDirection",str_where:"' + str_where + '"}';
            var rqurl = '/WebService.asmx/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            var item = Jsonstr.d;
            if (item == null) {
                return false;
            }
            for (var i = 0; i < item.length; i++) {
                options += "<option value=" + item[i] + ">" + item[i] + "</option>";
            }
            $(options).appendTo("#slToDirection_more")//
        }
        $("#txtRoadLines").val(lines.valueOf())
    });
    $("#slToDirection_more").change(function () {
        var ToDirection = $(this).val();
        if (ToDirection != "-开往-") {

            var RoadLines = $("#txtRoadLines").val().split(',');
            var isnodirection = false;
            var txtRoads = "";
            $(RoadLines).each(function (i) {
                var RoadInfos = RoadLines[i].split('-');
                if (i == RoadLines.length - 1) {
                    var NewRoadInfo = RoadInfos[0] + "-" + ToDirection;
                    txtRoads += NewRoadInfo;
                    $("#txtRoadLines").val(txtRoads);
                }
                else {
                    txtRoads += RoadLines[i] + ",";
                    $("#txtRoadLines").val(txtRoads);
                }

            });
        }
    });
    $("#hideResult").click(function () {
        if ($("#left").css("display") == "block") {
            var width = $(window).width() - $(this).width(); //document.documentElement.clientWidth - 10.5;
            var height = $(window).height() - 137 + 39; //document.documentElement.clientHeight - 125;
            $("#left").css("display", "none");
            $("#mapbar").removeClass("mapObj").addClass("mapObj2");
            $("#mapTitle").removeClass("title").addClass("title2");
            $("#hideResult").removeClass("hideResult").addClass("hideResult2");
            $(".right").css({ "width": width - 6 });
            $("#mapTitle").css({ "width": width - 6 });
            maplet.resize(width - 6, height);
            maplet.refresh();
        }
        else {
            var width = $(window).width() - $(this).width();
            var height = $(window).height() - 137+39;
            $("#mapbar").removeClass("mapObj2").addClass("mapObj");
            $("#mapTitle").removeClass("title2").addClass("title");
            $("#hideResult").removeClass("hideResult2").addClass("hideResult");
            $(".right").css({ "width": width - 340 - 6 });
            $("#mapTitle").css({ "width": width - 340 - 6 });
            //减去固定的宽度
            width = width - 340 - 6;
            // height = height - 2;
            maplet.resize(width, height);
            maplet.refresh();
            $("#left").css("display", "block");

        }
    });


   
    $("#slRoadName").change(function () {
        var RoadNane = $(this).val();
        if (RoadNane != "-路名-") {
            var txtRoads = $("#txtRoads").val();
            var Roads = txtRoads.split(',');

            if ($.inArray(RoadNane, Roads) == -1) {
                if (txtRoads != "") {
                    $("#txtRoads").val(txtRoads + "," + RoadNane);
                }
                else {
                    $("#txtRoads").val( RoadNane);
                }
            }
        }
    });
});
function removeAllOverlays() {
    maplet.clearOverlays(true);
    maplet.refresh();
}
//设置位置为中心点
function LatLonToCenter(strlatlon) {
    if (strlatlon == null || strlatlon == undefined || strlatlon == "") {
        window.alert("当前经纬度值为空或者无效！");
    }
    else {
        var lat = strlatlon.split(',')[0];
        var lon = strlatlon.split(',')[1]
        maplet.centerAndZoom(new MPoint(lat, lon), 18);
    }
}
//设置位置为中心点
function LatLonToCenter(lat, lon, showline) {
    var markers = maplet.getMarkers();
    for (var i = 0; i < markers.length; i++) {
        if (showline) {
            if (markers[i].pt.getPid() == lat + "," + lon) {
                if (i == 0) {
                    markers[i].setIcon(new MIcon("/Images/icons/起点.png", 24, 24), true);
                }
                else if (i == markers.length - 1) {
                    markers[i].setIcon(new MIcon("/Images/icons/终点.png", 24, 24), true);
                } else {
                    markers[i].setIcon(new MIcon("/Images/icons/1/icon_03.png", 16, 16), true);
                }
                markers[i].label.setVisible(true);
                //window.alert(markers[i].pt.getPid());
            }
            else {
                if (i == 0) {
                    markers[i].setIcon(new MIcon("/Images/icons/起点.png", 24, 24), true);
                }
                else if (i == markers.length - 1) {
                    markers[i].setIcon(new MIcon("/Images/icons/终点.png", 24, 24), true);
                }
                else {
                    markers[i].setIcon(new MIcon("/Images/icons/1/icon_03.png", 16, 16), true);
                }
                markers[i].label.setVisible(false);
            }
        }
        else {
            if (markers[i].pt.getPid() == lat + "," + lon) {

                //                markers[i].setIcon(new MIcon("/Images/icons/小蓝.png", 21, 22), true);
                markers[i].label.setVisible(true);
            }
            else {
                //                markers[i].setIcon(new MIcon("/Images/icons/小红.png", 21, 22), true);
                markers[i].label.setVisible(false);
            }
        }
        var new_shado = new MIconShadow("/Images/icons/shadow.png", 27, 34, -14)
        markers[i].setShadow(new_shado, true)
    }
    maplet.centerAndZoom(new MPoint(lat, lon), 11);


}
var mapEventListener;
// 下拉级联
function selectitem(item) {
    var itemtext = item.options[item.selectedIndex].value;
    var c_id = item.id;

    var District = $("#Distrtict").val();
    var RoadName = $("#RoadName").val();
    var StationName = $("#StationName").val();
    var PathDirection = $("#PathDirection").val();
    var tempoption = "<option value=''>-请选择-</option>";
    if (itemtext != "") {
        if (c_id == "Distrtict") {
            $("#RoadName option:not(:first)").remove();
            $("#StationName option:not(:first)").remove();
            $("#PathDirection option:not(:first)").remove();
            $("#StationAddress option:not(:first)").remove();

            var options = "";
            var str_where = "District='" + itemtext + "'";
            if (itemtext == "-区属-") {
                str_where = "(1=1)";
            }
            var jsondata = '{table_name:"tblStop",str_field:"RoadName",str_where:"' + str_where + '"}';
            var rqurl = '/WebService.asmx/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            var item = Jsonstr.d;
            if (item == null) {
                return false;
            }
            for (var i = 0; i < item.length; i++) {
                options += "<option value=" + item[i] + ">" + item[i] + "</option>";
            }
            $(options).appendTo("#RoadName")//
        }
        else if (c_id == "RoadName") {
            $("#StationName option:not(:first)").remove();
            $("#PathDirection option:not(:first)").remove();
            $("#StationAddress option:not(:first)").remove();

            var options = "";
            var str_where = " RoadName='" + itemtext + "'";
            var jsondata = '{table_name:"tblStop",str_field:"StationName",str_where:"' + str_where + '"}';
            var rqurl = '/WebService.asmx/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            var item = Jsonstr.d;
            if (item == null) {
                return false;
            }
            for (var i = 0; i < item.length; i++) {
                options += "<option value=" + item[i] + ">" + item[i] + "</option>";
            }
            $(options).appendTo("#StationName")//
        }
        else if (c_id == "StationName") {
            $("#PathDirection option:not(:first)").remove();
            $("#StationAddress option:not(:first)").remove();

            var options = "";
            var str_where = " RoadName='" + RoadName + "' and StationName='" + itemtext + "'";
            var jsondata = '{table_name:"tblStop",str_field:"PathDirection",str_where:"' + str_where + '"}';
            var rqurl = '/WebService.asmx/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            var item = Jsonstr.d;
            if (item == null) {
                return false;
            }
            for (var i = 0; i < item.length; i++) {
                options += "<option value=" + item[i] + ">" + item[i] + "</option>";
            }
            $(options).appendTo("#PathDirection")//
        }
        else if (c_id == "PathDirection") {
            $("#StationAddress option:not(:first)").remove();
            var options = "";
            var str_where = " RoadName='" + RoadName + "' and StationName='" + StationName + "' and PathDirection='" + itemtext + "'";
            var jsondata = '{table_name:"tblStop",str_field:"StationAddress",str_where:"' + str_where + '"}';
            var rqurl = '/WebService.asmx/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            var item = Jsonstr.d;
            if (item == null) {
                return false;
            }
            for (var i = 0; i < item.length; i++) {
                options += "<option value=" + item[i] + ">" + item[i] + "</option>";
            }
            $(options).appendTo("#StationAddress")//
        }
        else if (c_id == "slRoadLine") {
            $("#ToDirection option:not(:first)").remove();
            var options = "";
            var str_where = "RoadLine='" + itemtext + "'";
            var jsondata = '{table_name:"tblRoadBaseInfo",str_field:"ToDirection",str_where:"' + str_where + '"}';
            var rqurl = '/WebService.asmx/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            var item = Jsonstr.d;
            if (item == null) {
                return false;
            }
            for (var i = 0; i < item.length; i++) {
                options += "<option value=" + item[i] + ">" + item[i] + "</option>";
            }
            $(options).appendTo("#ToDirection")//
        }
    }
    else {

    }
}
//通过路线名称 行驶方向 查找路线
function SearchRoadLine() {
    clearMap();
    //如果查询结果部分是关闭的则在这里打开
    if ($("#left").css("display") != "block") {
        $("#hideResult").click();
    }
    // $("#sslk").attr("checked", false);
    var TotalDiv = ""; //统计数据
    //线路名
    var RoadLine = $("#slRoadLine").val();
    //方向
    var ToDirection = $("#ToDirection").val();
    if (RoadLine == "") {
        window.alert("请选择路线名!");
        return false;
    }
    if (ToDirection == "") {
        window.alert("请选择开往!");
        return false;
    }
    points = new Array();
    var jsondata = '{RoadLine:"' + RoadLine + '",ToDirection:"' + ToDirection + '"}';
    var rqurl = '/WebService.asmx/getRoadLineInfoByRoadLine';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var Jsonstr = $.parseJSON(callbackdata);
    var item = $.parseJSON(Jsonstr.d);
    if (item == null || item == "") {
        alert('当前路线不存在！');
        return false;
    }
    removeAllOverlays();
    globaldata = item;
    var tbl_temp = "";
    var k = 0;
    $("#result").hide();
    /*---将得到的地图数据放入表格中---*/
    tbl_temp += "<div style='cursor:pointer;'><table style='border-style: solid; border-width: 1px;border-color: #CCCCCC #999 #999 #CCCCCC;width:100%;text-align:left' "
                        + "onclick='javascript:{ if ($(\"#result" + k + "\").css(\"display\") == \"block\") {$(\"#result" + k + "\").hide();$(\"#img" + k
                        + "\").attr(\"src\",\"/images/plus.png\");}else{$(\"#result" + k + "\").show();$(\"#img" + k
                        + "\").attr(\"src\",\"/images/minus.png\");}};'><tr><th><img style='border:0px' src='/Resource/Images/plus.png' id='img" + k
                        + "' /></th><td>" + item.RoadLine + "开往:" + item.ToDirection + "(" + item.TicketStyle + ")</td></tr></table></div><div style='font-size:12px; display:block;' id='result" + k
                        + "'>" + getCreateHeader(item.RoadLine, item.StationPointInfos[0].SETime, item.RunCompany, item.ToDirection, item.TicketStyle, item.StationPointInfos.length) + "<table style='position:relative;left: 33px;font-size:12px;height:120%;top:0px;'  Id=\'TblMap" + k + "\'>";
    for (var j = 0; j < item.StationPointInfos.length; j++) {
        var strLatLon = item.StationPointInfos[j].StrLatlon;
        var labl = item.StationPointInfos[j].PointName; //标签点的名称
        var SETime = item.StationPointInfos[j].SETime; //首末班车时间
        var LineListleng = item.StationPointInfos[j].LineListleng;
        if (j == item.StationPointInfos.length - 1) {
            var RoadNum = item.RoadNum; //路线总数
            var BrandNum = item.BrandNum; //站牌总数
            var RoadCount = 0;
            if (item.RelationsRoadLine != null && item.RelationsRoadLine != "") {
                RoadCount = item.RelationsRoadLine.length
            }
            TotalDiv = "<table style='height:20px; float: left; margin-left: 8px;'><td>路线数：" + RoadNum + "</td><td> &nbsp;&nbsp;&nbsp;&nbsp;站牌数：" + BrandNum + "</td> <td> &nbsp;&nbsp;关联线路数：" + RoadCount + "</td></table>";
        }
        if (strLatLon != "") {
            var lat = item.StationPointInfos[j].StrLatlon.split(',')[0]; //经度
            var lon = item.StationPointInfos[j].StrLatlon.split(',')[1]; //纬度
            var point = new MPoint(lat, lon); //坐标
            marker = new MMarker(point, new MIcon("/images/icons/1/icon_03.png", 8, 8));
            marker.setLabel(new MLabel(labl), true); //添加标签
            marker.label.setVisible(false)//隐藏标签

            ////
            var lineList = item.StationPointInfos[j].LineList.split("、");
            var strlineList = "";
            for (var i = 0; i < lineList.length; i++) {
                if (strlineList != "") {
                    strlineList += "、<a href='javascript:;' style='text-decoration: none;' onclick='javascript:searchroadline(\"" + lineList[i] + "\",\"" + item.StationPointInfos[j].StopId + "\",\"" + strLatLon + "\")'>" + lineList[i] + "</a>"

                } else {
                    strlineList += "<a href='javascript:;' style='text-decoration: none;' onclick='javascript:searchroadline(\"" + lineList[i] + "\",\"" + item.StationPointInfos[j].StopId + "\",\"" + strLatLon + "\")'>" + lineList[i] + "</a>"
                }
            }
            var CaptionNumberInfo = getCaptionNumberInfo(item.StationPointInfos[j]);
//            var str_div = "<div class='RoundedCorner'>"
//                + "<table id='tablereg'><tr><td style='width:60px'>站点站址</td><td style='width:240px;'>" + item.StationPointInfos[j].Address + "</td></tr><tr><td>停靠线路</td><td>" + strlineList + "</td></tr><tr><td>站点编号</td><td><span href='javascript:;' onclick='javascript:linkztsystem(\"" + item.StationPointInfos[j].StopId + "\")'>" + item.StationPointInfos[j].StopId + "</span></td>"
//                + "<tr><td>资产编号</td><td>" + CaptionNumberInfo + "</td></tr>"
//                + "</tr><tr><td>站点照片</td><td><a href='javascript:;' onclick='javascript:openimage(\"" + item.StationPointInfos[j].StopId + "\")'>点击查看照片</a></td></tr></table>"
//                + "</p><b class='rbottom'><b class='r4'></b><b class='r3'></b><b class='r2'></b><b class='r1'></b></b><div style='display:none;'>[" + labl + "]<div></div>";
            //            marker.info = new MInfoWindow("当前站点:" + labl, str_div, "20px", "20px");

            var Facilitiestxt = "";
            var CaptionNumberIN = 0;
            if (item.StationPointInfos[j].FacilitiesSUM != undefined) {
                var Facs = item.StationPointInfos[j].FacilitiesSUM;
                $(Facs).each(function (k) {
                    var FacilitiesType = Facs[k].FacilitiesType;
                    var Numbers = Facs[k].Number;
                    if (FacilitiesType == "无电候车亭") {
                        if (Numbers != "0") {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                            CaptionNumberIN += Number(Numbers);
                        }
                        else {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                        }
                    }
                    if (FacilitiesType == "有电候车亭") {
                        if (Numbers != "0") {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                            CaptionNumberIN += Number(Numbers);
                        }
                        else {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                        }

                    }
                    if (FacilitiesType == "视频") {
                        if (Numbers != "0") {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        }
                        else {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                        }
                    }
                    if (FacilitiesType == "灯片") {
                        if (Numbers != "0") {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        }
                        else {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                        }

                    }
                    if (FacilitiesType == "立杆") {
                        if (Numbers != "0") {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' />" + Numbers + "&nbsp;&nbsp;";
                            CaptionNumberIN += Number(Numbers);
                        }
                        else {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' />&nbsp;&nbsp;";
                        }
                    }
                });
            }


            var height = 228;
            //设施数量
            var CaptionNumberManager = 0
            if (CaptionNumberIN > 4) {
                CaptionNumberManager = 15;
            }
            //站址
            var byteLen = byteLength(item.StationPointInfos[j].Address);
            var byteLenManager = 0;
            if (byteLen > 36) {
                byteLenManager = 15;
            }

            if (LineListleng <= 35) {
                height = 150 + CaptionNumberManager + byteLenManager;
            }
            if (LineListleng > 35 && LineListleng <= 68) {
                height = 168 + CaptionNumberManager + byteLenManager;
            }
            if (LineListleng > 68 && LineListleng <= 102) {
                height = 185 + CaptionNumberManager + byteLenManager;
            }
            if (LineListleng > 102 && LineListleng <= 125) {
                height = 201 + CaptionNumberManager + byteLenManager;
            }
            //height -= 20;
            var str_div = "<div id='RoundedCorner" + j + "' style='height:" + height + "px;width:280px;border-top:1px solid #7b9ebd;background-image:url(/Resource/images/imgs/03.png);border-left:1px solid #008d44;border-right:1px solid #008d44;margin-top:105px; margin-left:1px'>";
            str_div += "<div style=' width:100%; height:30px; z-index:100; background-image:url(/Resource/images/imgs/1.jpg);font-size:12px; line-height:30px; color:#fff; '><span style=' float:left; padding-left:10px'>公交站点：" + labl + "</span><img src='/Resource/Images/imgs/close.jpg' style=' float:right;text-decoration:none;margin-top:5px;cursor:pointer' onclick='colse(RoundedCorner" + j + ")'/></div>";

            str_div += " <div>"
            str_div += "<table style='font-size: 12px'><tr><td style='width:60px'>站点站址</td><td style='width:220px;'>" + item.StationPointInfos[j].Address + "</td></tr><tr><td>停靠线路</td><td style='width:220px;'>" + strlineList + "</td></tr>";
            str_div += "<tr><td>站点编号</td><td><span href='javascript:;' onclick='javascript:linkztsystem(\"" + item.StationPointInfos[j].StopId + "\")'>" + item.StationPointInfos[j].StopId + "</span></td></tr>";
            str_div += "<tr><td>资产编号</td><td style='width:220px;'>" + CaptionNumberInfo + "</td></tr>";
            str_div += "<tr><td>站点照片</td><td><a href='javascript:;' onclick='javascript:openimage(\"" + item.StationPointInfos[j].StopId + "\")'>点击查看照片</a></td></tr>";
            str_div +="</table>";
            str_div += "<div style='display:none;'>[" + labl + "]</div></div>";
            if (Facilitiestxt != "" && Facilitiestxt != null && Facilitiestxt != undefined) {
                str_div += "<div style='height:20px; font-size: 15px;'>" + Facilitiestxt + "</div>";
            }
            else {
                str_div += "<div style='height:24px; font-size: 15px;'></div>";
            }

            str_div += "<div style='width:100%; background-image:url(/Resource/images/imgs/0300_02.png); height:69px;overflow:hidden; background-position:-10px' ></div></div>";


            var m_window = new MInfoWindow("11", str_div);
            marker.info = m_window;
            maplet.addOverlay(marker);
            MEvent.addListener(marker, "iw_beforeopen", showCustomIw);




            var linkecolor = "Black";
            var backgroundimage = "";
            if (j == 0) {
                marker.setIcon(new MIcon("/images/icons/起点.png", 16, 16), true);
                linkecolor = "Red";
                backgroundimage = "background-image: url(\"/images/point.png\");background-repeat: no-repeat;";
            }
            else if (j == item.StationPointInfos.length - 1) {
                marker.setIcon(new MIcon("/images/icons/终点.png", 16, 16), true);
                linkecolor = "Blue";
            }
            tbl_temp += "<tr><td style=\'width:12px;" + backgroundimage + " \'></td><td style=\'width:12px; \'></td><td style='color:Blue'><a href='javascript:LatLonToCenter(" + strLatLon + ",false);javascript:ToPoint(TblMap" + k + "," + j + ");' style='text-decoration: none;color:" + linkecolor + "'>" + labl + "</a><input type='hidden'  value='" + strLatLon + "'></td><td >" + SETime + ".</td></tr>";

            MEvent.addListener(marker, "click", function (omarker) {
                omarker.openInfoWindow();
            });
            MEvent.addListener(marker, "mouseover", function (mk) {
                if (mk.label) mk.label.setVisible(true);
            })
            MEvent.addListener(marker, "mouseout", function (mk) {
                if (mk.label) mk.label.setVisible(false);
            })
            points.push(marker);
            if (k == 0) {
                maplet.addOverlay(marker);
            }
        }
        else {
            tbl_temp += "<tr><td style=\'width:12px;\'></td><td style=\'width:12px;\'></td><td style='color:Green'>" + labl + "</td><td >" + SETime + ".</td></tr>";
        }
    }

    tbl_temp += "<tr><td colspan=3 style='height:80px;'></tr>";
    tbl_temp += "</table></div>";
    tbl_temp = TotalDiv + tbl_temp;
    //用于存放当前点的集合的数组
    var strlinelatlon = "";
    if (item.LineLatLon != undefined) {

        var points_temp = new Array();
        var LineLatLon = item.LineLatLon.split(';')
        for (var i = 0; i < LineLatLon.length; i++) {

            if (LineLatLon[i] != null && LineLatLon[i] != "" && LineLatLon[i] != undefined) {
                if (i > 0) {
                    strlinelatlon += ";" + LineLatLon[i];
                }
                else {
                    strlinelatlon += LineLatLon[i];
                }
                var lat = LineLatLon[i].split(',')[0];
                var lon = LineLatLon[i].split(',')[1];
                var point = new MPoint(lat, lon);
                points_temp.push(point);
            }
        }
        //将地图移动到中心位置
        var lat = item.CenterPoint.split(',')[0];
        var lon = item.CenterPoint.split(',')[1]
        maplet.centerAndZoom(new MPoint(lat, lon), 10);
        $("#linecenter").val( item.CenterPoint);
        //创建笔刷将所有的点连接起来
        var brush = new MBrush();
        brush.arrow = false;
        brush.stroke = 4;
        brush.fill = false;
        brush.color = 'blue';
        brush.bgcolor = 'red';
        var polyline = new MPolyline(points_temp, brush);
        maplet.addOverlay(polyline);
        //注释代码为线路轨迹回放代码
        //lines = item.LineLatLon.split(';');
        //startORI();

    }
    $("#roadlines").val(strlinelatlon);

    $("#resultcontent").html(tbl_temp);
    //显示 站名
    showstationname(-1); ;
}
function openimage(stopid) {
    window.open("/ReportNew/showImages.aspx?StopId=" + stopid);
}
//通过站点查找路线
function SearchRoadLineByPoint() {
    clearMap();
    points = new Array();
    var District = $("#Distrtict").val();
    var RoadName = $("#RoadName").val();
    var StationName = $("#StationName").val();
    var PathDirection = $("#PathDirection").val();
    var StationAddress = $("#StationAddress").val();
    var TotalDiv = ""; //统计数据

    if (RoadName == "") {
        window.alert("请选择路名");
        return false;
    }
    if (StationName == "") {
        window.alert("请选择站名");
        return false;
    }
    if (PathDirection == "") {
        window.alert("请选择车向");
        return false;
    }
    if (StationAddress == "") {
        window.alert("请选择站址");
        return false;
    }
    //如果查询结果部分是关闭的则在这里打开
    if ($("#left").css("display") != "block") {
        $("#hideResult").click();
    }
    var jsondata = '{District:"' + District + '",RoadName:"' + RoadName + '",StationName:"' + StationName + '",PathDirection:"' + PathDirection + '",StationAddress:"' + StationAddress + '"}';
    var rqurl = '/WebService.asmx/getRoadLineListByDB';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var Jsonstr = eval('(' + callbackdata + ')');
    var items = Jsonstr.d;
    if (items == null) {
        return false;
    }
    removeAllOverlays();
    globaldata = items;
    var tbl_temp = "";
    for (var k = 0; k < items.length; k++) {
        var item = items[k];
        $("#result").hide();
        /*---请求地图数据end ---*/
        /*---将得到的地图数据放入表格中---*//// <reference path="../image/TabBak.png" />
        /// <reference path="../image/minus.png" />
        tbl_temp += "<div style='cursor:pointer;'><table style='border-style: solid; border-width: 1px;border-color: #CCCCCC #999 #999 #CCCCCC;width:100%;text-align:left' "
                        + "onclick='javascript:{ if ($(\"#result" + k + "\").css(\"display\") == \"block\") {$(\"#result" + k + "\").hide();$(\"#img" + k
                        + "\").attr(\"src\",\"/images/plus.png\");removeAllOverlays();}else{$(\"#result" + k + "\").show();$(\"#img" + k
                        + "\").attr(\"src\",\"/images/minus.png\");DrawingLines(\"" + k + "\");}};'><tr><th><img style='border:0px' src='/Resource/Images/plus.png' id='img" + k
                        + "' /></th><td>" + item.RoadLineInfo + "（" + item.TicketStyle + ")</td></tr></table></div><div style='font-size:12px; display:none;' id='result" + k
                         + "'><table style='position:relative;left: 33px;font-size:12px;height:120%;top:0px;'  Id=\'TblMap" + k + "\'>";
        for (var j = 0; j < item.StationPointInfos.length; j++) {
            var strLatLon = item.StationPointInfos[j].StrLatlon;
            var LineListleng = item.StationPointInfos[j].LineListleng;
            var lat = item.StationPointInfos[j].StrLatlon.split(',')[0]; //经度
            var lon = item.StationPointInfos[j].StrLatlon.split(',')[1]; //纬度
            var labl = item.StationPointInfos[j].PointName; //标签点的名称
            var SETime = item.StationPointInfos[j].SETime; //首末班车时间
            if (j == item.StationPointInfos.length - 1 && k == items.length - 1) {
                var RoadNum = item.RoadNum; //路线总数
                var BrandNum = item.BrandNum; //站牌总数
                TotalDiv = "<div><table style='height:40px; float: left; margin-left: 8px;'><td>路线数：" + RoadNum + "</td><td> &nbsp;&nbsp;&nbsp;&nbsp;站牌数：" + BrandNum + "</td></table></div>";
            }
            if (strLatLon != "") {
                var point = new MPoint(lat, lon); //坐标
                if (j == 0) {
                    marker = new MMarker(point, new MIcon("/images/icons/起点.png", 16, 16));
                }
                else if (j == (item.StationPointInfos.length - 1)) {
                    marker = new MMarker(point, new MIcon("/images/icons/终点.png", 16, 16));
                }
                else {
                    marker = new MMarker(point, new MIcon("/images/icons/1/icon_03.png", 8, 8));
                }
                marker.setLabel(new MLabel(labl), true); //添加标签
                marker.label.setVisible(false)//隐藏标签
                var lineList = item.StationPointInfos[j].LineList.split("、");
                var strlineList = "";
                for (var i = 0; i < lineList.length; i++) {
                    if (strlineList != "") {
                        strlineList += "、<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[i] + "\",\"" + item.StationPointInfos[j].StopId + "\",\"" + strLatLon + "\")'>" + lineList[i] + "</a>"

                    } else {
                        strlineList += "<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[i] + "\",\"" + item.StationPointInfos[j].StopId + "\",\"" + strLatLon + "\")'>" + lineList[i] + "</a>"
                    }
                }
                var CaptionNumberInfo = getCaptionNumberInfo(item.StationPointInfos[j]);

                var Facilitiestxt = "";
                var CaptionNumberIN = 0;
                if (item.StationPointInfos[j].FacilitiesSUM != undefined) {
                 var Facs = item.StationPointInfos[j].FacilitiesSUM;
                 $(Facs).each(function (k) {
                     var FacilitiesType = Facs[k].FacilitiesType;
                     var Numbers = Facs[k].Number;
                     if (FacilitiesType == "无电候车亭") {
                         if (Numbers != "0") {
                             Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                             CaptionNumberIN += Number(Numbers);
                         }
                         else {
                             Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                         }
                     }
                     if (FacilitiesType == "有电候车亭") {
                         if (Numbers != "0") {
                             Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                             CaptionNumberIN += Number(Numbers);
                         }
                         else {
                             Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                         }
                     }
                     if (FacilitiesType == "视频") {
                         if (Numbers != "0") {
                             Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                         }
                         else {
                             Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                         }
                     }
                     if (FacilitiesType == "灯片") {
                         if (Numbers != "0") {
                             Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                         }
                         else {
                             Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                         }
                     }
                     if (FacilitiesType == "立杆") {
                         if (Numbers != "0") {
                             Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                             CaptionNumberIN += Number(Numbers);
                         }
                         else {
                             Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                         }
                     }
                 });
             }
             var height = 228;
             //设施数量
             var CaptionNumberManager = 0
             if (CaptionNumberIN > 4) {
                 CaptionNumberManager = 15;
             }
             //站址
             var byteLen = byteLength(item.StationPointInfos[j].Address);
             var byteLenManager = 0;
             if (byteLen > 36) {
                 byteLenManager = 15;
             }

             if (LineListleng <= 35) {
                 height = 150 + CaptionNumberManager + byteLenManager;
             }
             if (LineListleng > 35 && LineListleng <= 68) {
                 height = 168 + CaptionNumberManager + byteLenManager;
             }
             if (LineListleng > 68 && LineListleng <= 102) {
                 height = 185 + CaptionNumberManager + byteLenManager;
             }
             if (LineListleng > 102 && LineListleng <= 125) {
                 height = 201 + CaptionNumberManager + byteLenManager;
             }
            
             var str_div = "<div id='RoundedCorner" + j + "' style='height:" + height + "px;width:280px;border-top:1px solid #7b9ebd;background-image:url(/Resource/images/imgs/03.png);border-left:1px solid #008d44;border-right:1px solid #008d44;margin-top:105px; margin-left:1px'>";
             str_div += "<div style=' width:100%; height:30px; z-index:100; background-image:url(/Resource/images/imgs/1.jpg);font-size:12px; line-height:30px; color:#fff; '><span style=' float:left; padding-left:10px'>公交站点：" + labl + "</span><img src='/Resource/Images/imgs/close.jpg' style=' float:right;text-decoration:none;margin-top:5px;cursor:pointer' onclick='colse(RoundedCorner" + j + ")'/></div>";

                str_div += " <div>"
                str_div += "<table style='font-size: 12px'><tr><td style='width:60px'>站点站址</td><td style='width:220px;'>" + item.StationPointInfos[j].Address + "</td></tr><tr><td>停靠线路</td><td style='width:220px;'>" + strlineList + "</td></tr>";
                str_div += "<tr><td>站点编号</td><td><span href='javascript:;' onclick='javascript:linkztsystem(\"" + item.StationPointInfos[j].StopId + "\")'>" + item.StationPointInfos[j].StopId + "</span></td></tr>";
                str_div += "<tr><td>资产编号</td><td style='width:220px;'>" + CaptionNumberInfo + "</td></tr>";
                str_div += "<tr><td>站点照片</td><td><a href='javascript:;' onclick='javascript:openimage(\"" + item.StationPointInfos[j].StopId + "\")'>点击查看照片</a></td></tr>";
                str_div += " </table>";
                str_div += "<div style='display:none;'>[" + labl + "]</div></div>";

                if (Facilitiestxt != "" && Facilitiestxt != null && Facilitiestxt != undefined) {
                    str_div += "<div style='height:20px; font-size: 15px;'>" + Facilitiestxt + "</div>";
                }
                else {
                    str_div += "<div style='height:24px; font-size: 15px;'></div>";
                }

                str_div += "<div style='width:100%; background-image:url(/Resource/images/imgs/0300_02.png); height:69px;overflow:hidden; background-position:-10px' ></div></div>";


                var m_window = new MInfoWindow("11", str_div);
                marker.info = m_window;
                maplet.addOverlay(marker);
                MEvent.addListener(marker, "iw_beforeopen", showCustomIw);

                tbl_temp += "<tr><td style='color:Blue'><a href='javascript:LatLonToCenter(" + strLatLon + ",false);' style='text-decoration: none;'>" + labl + "</a><input type='hidden'  value='" + strLatLon + "'></td><td >" + SETime + ".</td></tr>";
                //marker.bEditable = true;
                // 添加对marker 的事件
                MEvent.addListener(marker, "click", function (omarker) {
                    omarker.openInfoWindow();
                });
                MEvent.addListener(marker, "mouseover", function (mk) {
                    if (mk.label) mk.label.setVisible(true);
                })
                MEvent.addListener(marker, "mouseout", function (mk) {
                    if (mk.label) mk.label.setVisible(false);
                })
                points.push(marker);
                maplet.addOverlay(marker);
            } else {
                tbl_temp += "<tr><td style='color:Green'>" + labl + "<input type='hidden'  value='" + strLatLon + "'></td><td >" + SETime + ".</td></tr>";
            }

        }
        tbl_temp += "</table></div>";
        tbl_temp = TotalDiv + tbl_temp;
        // $("#resultcontent").html(tbl_temp);
        //将所有经纬度添加到集合中去
        //用于存放当前点的集合的数组
        var strlinelatlon = "";
        var points_temp = new Array();
        for (var i = 0; i < item.LineLatLon.length; i++) {

            if (item.LineLatLon[i] != null && item.LineLatLon[i] != "" && item.LineLatLon != undefined) {
                if (i > 0) {
                    strlinelatlon += ";" + item.LineLatLon[i];
                }
                else {
                    strlinelatlon += item.LineLatLon[i];
                }
                var lat = item.LineLatLon[i].split(',')[0];
                var lon = item.LineLatLon[i].split(',')[1];
                var point = new MPoint(lat, lon);
                points_temp.push(point);
            }
        }
        //将地图移动到中心位置
        var lat = item.CenterPoint.split(',')[0];
        var lon = item.CenterPoint.split(',')[1]
        maplet.centerAndZoom(new MPoint(lat, lon), 13);
        $("#linecenter").attr("value", item.CenterPoint);
        //创建笔刷将所有的点连接起来
        var brush = new MBrush();
        brush.arrow = false;
        brush.stroke = 5;
        brush.fill = false;
        brush.color = getRoadLineColor(k);
        var polyline = new MPolyline(points_temp, brush);
        maplet.addOverlay(polyline);

        $("#roadlines").val(strlinelatlon);
    }
    $("#resultcontent").html(tbl_temp);

    //显示 站名
    showstationname(-1); ;
}
function randomColor() {
    var n = Math.floor(Math.random() * 16);
    switch (n) {
        case 10: c = "a"; break;
        case 11: c = "b"; break;
        case 12: c = "c"; break;
        case 13: c = "d"; break;
        case 14: c = "e"; break;
        case 15: c = "f"; break;
        default: c = n; break;
    }
    sc = c.toString();
    return (sc);
}
//查找 站点
var PointsTr = "";
var PointCount = 0;
var SqlArry = new Array();

var mk = null;
function SearchPoints() {
    clearMap();
    points = new Array();
    var Area = $("#Area").val();
    var District = $("#Distrtict").val();
    var RoadName = $("#RoadName").val();
    var StationName = $("#StationName").val();
    var PathDirection = $("#PathDirection").val();
    var StationAddress = $("#StationAddress").val();
    var Station = $("#cbxStation").prop("checked");
    var YD = $("#cbxYD").attr("checked");
    var WD = $("#cbxWD").attr("checked");
    var Pole = $("#cbxPole").prop("checked");
    var Lcdtxt = $("#cbxLcd").attr("checked");
    var Lamptxt= $("#cbxLamp").attr("checked");
    var Terminus = $("#cbxTerminus").attr("checked");
    var Terminal = $("#cbxTerminal").attr("checked");
    var OnWay = $("#cbxOnWay").attr("checked");
    var shelter = "";
    var strwhere = "";
    var Lcd = "";
    var Lamp = "";
    var Electricity = "";
    if (RoadName == "-路名-" || RoadName == "") {
        if (Area != "-环域-" || District != "-区属-") {
            if (Area != "-环域-") {
                strwhere += " Area='" + Area + "'";
            }
            if (District != "-区属-") {
                if (strwhere == "") {
                    strwhere += " District='" + District + "'";
                }
                else {
                    strwhere += " AND District='" + District + "'";
                }
            }
        }
        else {
            alert("未选择任何查询条件!")
            return false;
        }
    }
    else {

        strwhere += " RoadName='" + RoadName + "'"
        if (StationName != "") {
            strwhere += " and StationName='" + StationName + "'";
        }
        if (PathDirection != "") {
            strwhere += " and PathDirection='" + PathDirection + "'";
        }
        if (StationAddress != "") {
            strwhere += " and StationAddress='" + StationAddress + "'";
        }
    }

    if (Station) {
        shelter = "Station";
        if (YD) {
            Electricity == "有电"
        }
        if (WD) {
            Electricity == "无电"
        }
    }
    if (Pole) {
        shelter = "Pole";
    }
    if (Lcdtxt) {
       Lcd="Lcd"
   }
   if (Lamptxt) {
       Lamp = "Lamp"
   }
    if (Station && Pole) {
        shelter = "All";
    }
    var strCdt = "";
    if (Terminus) {
        if (strCdt == "") {
            strCdt = "PathDirection Like '终点%'";
        }
        else {
            strCdt += " OR PathDirection Like '终点%'";
        }
    }
    if (Terminal) {
        if (strCdt == "") {
            strCdt = "PathDirection Like '枢纽%'";
        }
        else {
            strCdt += " OR PathDirection Like '枢纽%'";
        }
    }
    if (OnWay) {
        if (strCdt == "") {
            strCdt = "pathdirection not like '终点%' and pathdirection not like '枢纽%'";
        }
        else {
            strCdt += " OR (pathdirection not like '终点%' and pathdirection not like '枢纽%')";
        }
    }
    if (strCdt != "") {
        strCdt = " and (" + strCdt + " )";
        strwhere = strwhere + strCdt;
    }
    //如果查询结果部分是关闭的则在这里打开
    if ($("#left").css("display") != "block") {
        $("#hideResult").click();
    }
    var jsondata = '{strWhere:"' + strwhere + '",Flag:"' + shelter + '",Lcd:"' + Lcd + '",Lamp:"' + Lamp + '",Electricity:"' + Electricity + '"}';
    var strWheresql='strWhere:"' + strwhere + '",Flag:"' + shelter + '",Lcd:"' + Lcd + '",Lamp:"' + Lamp + '",Electricity:"' + Electricity + '"';
    if (!$("#cbxIsAdd").attr("checked")) {
        PointsTr = "";
        removeAllOverlays();
        SqlArry = new Array();
        SqlArry.push(jsondata);
    }
    else {
        if ($.inArray(jsondata, SqlArry) > -1) {
            alert("累计查询结果的情况下无法添加相同的查询条件！");
            return false;
        }
        else {
            SqlArry.push(jsondata);
        }
    }
    var rqurl = '/WebService.asmx/getPonintInfo';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var Jsonstr = $.parseJSON(callbackdata);
    var item = $.parseJSON(Jsonstr.d);
    if (item == null) {
        return false;
    }
    points = new Array();
    /*---请求地图数据end ---*/
    /*---将得到的地图数据放入表格中---*/
    var RoadlineCount ="";
    var StationCount  ="";
    var PolesCount    ="";
    var LcdCount      ="";
    var LampCount = "";
    var StopIdlist = "";

    for (var i = 0; i < item.length; i++) {
        var marker = null;
        var labl = item[i].StationName; //标签点的名称
        if (StopIdlist.length > 0) {
            StopIdlist += "," + item[i].StopId;
        } else {
            StopIdlist += item[i].StopId;
        }
        var strLatLon = item[i].Strlatlon;
        if (strLatLon != null && strLatLon != "") {
            var lat = strLatLon.split(',')[0]; //经度
            var lon = strLatLon.split(',')[1]; //纬度
            var point = new MPoint(lat, lon); //坐标点
            var PathDirection = item[i].PathDirection
            var w = 16;
            var h = 16;
            var icourl = "/images/icons/1/icon_03.png";
            switch (PathDirection) {
                case "枢纽":
                    icourl = "/images/icons/1/icon_01.png";
                    w = 40;
                    h = 32;
                    break;
                case "枢纽站":
                    w = 40;
                    h = 32;
                    icourl = "/images/icons/1/icon_01.png";
                    break;
                case "终点":
                    w = 24;
                    h = 24;
                    icourl = "/images/icons/1/icon_02.png";
                    break;
                case "终点站":
                    w = 24;
                    h = 24;
                    icourl = "/images/icons/1/icon_02.png";
                    break;
                default:
                    icourl = "/images/icons/1/icon_03.png";
                    break;
            }
            marker = new MMarker(point, new MIcon(icourl, w, h));
        }
        //        var new_shado = new MIconShadow("/images/icons/shadow.png", 16, 16, -14)
        //        marker.setShadow(new_shado, true)
        var lineList = item[i].LineList.split("、");
        var strlineList = "";

        for (var j = 0; j < lineList.length; j++) {
            if (strlineList != "") {
                strlineList += "、<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[j] + "\",\"" + item[i].StopId + "\",\"" + strLatLon + "\")'>" + lineList[j] + "</a>"
                
            } else {
                strlineList += "<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[j] + "\",\"" + item[i].StopId + "\",\"" + strLatLon + "\")'>" + lineList[j] + "</a>"
            }
        }
        
        var LineListleng = item[i].LineListleng;
         RoadlineCount = item[i].RoadlineCount;
         StationCount  = item[i].StationCount;
         PolesCount    = item[i].PolesCount;
         LcdCount      = item[i].LcdCount;
         LampCount = item[i].LampCount;
        var CaptionNumberIN= 0;
        var CaptionNumberInfo = getCaptionNumberInfo(item[i]);
        var Facilitiestxt = "";
        if (item[i].FacilitiesSUM != undefined) {
            var Facs = item[i].FacilitiesSUM;
            $(Facs).each(function (k) {
                var FacilitiesType = Facs[k].FacilitiesType;
                var Numbers= Facs[k].Number;
                if (FacilitiesType == "无电候车亭") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        CaptionNumberIN += Number(Numbers);
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "有电候车亭") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        CaptionNumberIN += Number(Numbers);
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "视频") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "灯片") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "立杆") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        CaptionNumberIN += Number(Numbers);
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
            });
        }
        var height = 228;
        //设施数量
        var CaptionNumberManager = 0
        if (CaptionNumberIN > 4) {
            CaptionNumberManager = 15;
        }
        //站址
        var byteLen = byteLength(item[i].StationAddress);
        var byteLenManager = 0;
        if (byteLen > 36) {
            byteLenManager = 15;
        }

        if (LineListleng <= 35) {
            height = 145 + CaptionNumberManager + byteLenManager;
        }
        if (LineListleng > 35 && LineListleng <=68) {
            height = 163 + CaptionNumberManager + byteLenManager;
        }
        if (LineListleng > 68 && LineListleng <= 102) {
            height = 180 + CaptionNumberManager + byteLenManager;
        }
        if (LineListleng > 102 && LineListleng <= 125) {
            height = 196 + CaptionNumberManager + byteLenManager;
        }
        ////height -= 20;

        var str_div = "<div id='RoundedCorner" + i + "' style='height:" + height + "px;width:280px;border-top:1px solid #7b9ebd;background-image:url(/Resource/images/imgs/03.png);border-left:1px solid #008d44;border-right:1px solid #008d44;margin-top:105px; margin-left:1px'>";
        str_div += "<div style=' width:100%; height:30px; z-index:100; background-image:url(/Resource/images/imgs/1.jpg);font-size:12px; line-height:30px; color:#fff; '><span style=' float:left; padding-left:10px'>公交站点：" + labl + "</span><img src='/Resource/Images/imgs/close.jpg' style=' float:right;text-decoration:none;margin-top:5px;cursor:pointer' onclick='colse(RoundedCorner" + i + ")'/></div>";
       
        str_div += " <div >"
        str_div += "<table style='font-size: 12px;'><tr><td style='width:60px'>站点站址</td><td style='width:220px;'>" + item[i].StationAddress + "</td></tr>";
        str_div += "<tr><td>停靠线路</td><td >" + strlineList + "</td></tr>";
        str_div += "<tr><td>站点编号</td><td ><span href='javascript:;' onclick='javascript:linkztsystem(\"" + item[i].StopId + "\")'>" + item[i].StopId + "</span></td></tr>";
        str_div += "<tr><td>资产编号</td><td>" + CaptionNumberInfo + "</td></tr>";
        str_div += "<tr><td>站点照片</td><td ><a href='javascript:;' onclick='javascript:openimage(\"" + item[i].StopId + "\")'>点击查看照片</a></td></tr>";
        str_div += "</table>";
        str_div += "<div style='display:none;'>[" + labl + "]</div></div>";
        if (Facilitiestxt != "" && Facilitiestxt != null && Facilitiestxt != undefined) {
            str_div += "<div style='height:20px; font-size: 15px;'>" + Facilitiestxt + "</div>";
        }
        else {
            str_div += "<div style='height:24px; font-size: 15px;'></div>";
        }

        str_div += "<div style='width:100%; background-image:url(/Resource/images/imgs/0300_02.png); height:69px;overflow:hidden; background-position:-10px' ></div></div>";

        

        if (strLatLon != null && strLatLon != "") { 
            var m_window = new MInfoWindow("11",str_div);
            marker.info = m_window;
            marker.setLabel(new MLabel(labl), true); //添加标签
            marker.label.setVisible(false)//隐藏标签

            maplet.addOverlay(marker);
            MEvent.addListener(marker, "iw_beforeopen", showCustomIw);
        }
        var index = 0;
        if (!$("#cbxIsAdd").attr("checked")) {
            index = (i + 1);
        }
        else {
            index = PointCount + (i + 1)
        }
        PointsTr += "<tr><td>" + index + ".<a href='javascript:LatLonToCenter(" + strLatLon + ",false)'>" + item[i].StationName + item[i].StationAddress + "</a></td></tr>"
        //marker.bEditable = true;
        // 添加对marker 的事件
        if (strLatLon != null && strLatLon != "") {
            MEvent.addListener(marker, "click", function (omarker) {
                omarker.openInfoWindow();
            });
            //omarker.
            MEvent.addListener(marker, "mouseover", function (mk) {
                if (mk.label) mk.label.setVisible(true);
            })
            MEvent.addListener(marker, "mouseout", function (mk) {
                if (mk.label) mk.label.setVisible(false);
            })
            points.push(marker);
            maplet.addOverlay(marker);
        }
        if (strLatLon != null && strLatLon != "") {
            //将地图移动到中心位置
//            maplet.centerAndZoom(point, 20);
            if (i == parseInt(item.length / 2)) {
                maplet.centerAndZoom(point, 20);
            }
        }
    }
    if (!$("#cbxIsAdd").attr("checked")) {
        PointCount = item.length;
    }
    else {
        PointCount += item.length;
    }
    var tbl_temp = "<div style='height:30px;top;20px; margin-left: 8px;'><table ><tr><td>站点数:" + PointCount+";";
    if(RoadlineCount!=null&&RoadlineCount!=undefined&&RoadlineCount!=""&&RoadlineCount!="0") {
        //tbl_temp += "线路数:<a href='javascript:;' onclick='javascript:openimageRoadline(\"" + StopIdlist + "\")'>" + RoadlineCount + "</a>;";
        tbl_temp += "线路数:" + RoadlineCount + ";";
    }
    if(StationCount!=null&&StationCount!=undefined&&StationCount!=""&&StationCount!="0")
    {
       tbl_temp+="候车亭数:" + StationCount + ";";
    }
    if(PolesCount!=null&&PolesCount!=undefined&&PolesCount!=""&&PolesCount!="0")
    {
        tbl_temp += "立杆数:" + PolesCount + ";";
    }
    if(LcdCount!=null&&LcdCount!=undefined&&LcdCount!=""&&LcdCount!="0")
    {
        tbl_temp += "视频:<a href='javascript:;' onclick='javascript:openimageLcd(\"" + StopIdlist + "\")'>" + LcdCount + "</a>;";
    }
    if(LampCount!=null&&LampCount!=undefined&&LampCount!=""&&LampCount!="0")
    {
        tbl_temp += "灯片:<a href='javascript:;' onclick='javascript:openimageLamp(\"" + StopIdlist + "\")'>" + LampCount + "</a>;";
    }
    tbl_temp+="</td></tr></table></div><div style='font-size:12px;'><table  Id='TblMap'>";
    tbl_temp += PointsTr;
    tbl_temp += "</table></div>";
    $("#resultcontent").html(tbl_temp);
    //显示 站名
    showstationname(-1); ;
}


function openimageRoadline(strWheresql) {
    window.open("RunCompanyRoadtowNew.aspx?StopIdList=" + strWheresql);
}
function openimageLcd(strWheresql) {
    window.open("LcdBaseInfo.aspx?StopIdList=" + strWheresql);
}
function openimageLamp(strWheresql) {
    window.open("LampBaseInfo.aspx?StopIdList=" + strWheresql);
}

function clearMap() {

    if (p != null) {
        maplet.removePanel(p, true);
    }
    maplet.customInfoWindow = true;
    vpoint = {};
    maplet.clearOverlays(true);
    maplet.refresh();
 
}
//function showCustomIw(overlay, x, y) {
//    var options = {
//        pin: true,
//        zindex: 5,
//        content: overlay.info.content,
//        location: { type: "xy", x: x, y: y },
//        zoomhide: true
//    };
//    p = new MPanel(options);
//    MPanel.enableDragMap(p.dom, true);
//    maplet.addPanel(p);
//}

//计算字节数
function byteLength(str) {
    var byteLen = 0, len = str.length;
    if (!str) return 0;
    for (var i = 0; i < len; i++)
        byteLen += str.charCodeAt(i) > 255 ? 2 : 1;
    return byteLen;
}




var p = null;
function showCustomIw(overlay, x, y) {
    if (p != null) {
        maplet.removePanel(p, true);
    }
    openv = overlay.info.title;
    mk = overlay;
    var showY = 0;
    var height = overlay.info.content.substring(overlay.info.content.indexOf(":") + 1, overlay.info.content.indexOf("px"));
    showY = 228 - height;
    
    var options = {
        pin: true,
        zindex: 5,
        content: overlay.info.content,
        location: { type: "xy", x: x - 170, y: y - 400 + showY },
        zoomhide: true
    };
    if (x < 300 && y < 350) {
        maplet.panTo(300 - x, 350 - y );
    }
    else if (y < 350) {
        maplet.panTo(0, 350 - y);
    }
    else if (x < 300) {
        maplet.panTo(300 - x, 0);
    }
    else if (x > $(window).width() - 100) {
        maplet.panTo(-250, 0);
    }

    p = new MPanel(options);
    MPanel.enableDragMap(p.dom, true);
    maplet.addPanel(p);
}

function StationChange() {
    var Station = $("#cbxStation").attr("checked");
    if (!Station) {
        //$("#cbxYD")[0].checked = false;
        //$("#cbxWD")[0].checked = false;
      //  $("#cbxLcd")[0].checked = false;
        //$("#cbxLamp")[0].checked = false;
    }
    var YD = $("#cbxYD").attr("checked");
    if (!YD) {
        //$("#cbxLcd")[0].checked = false;
        //$("#cbxLamp")[0].checked = false;
    }
}



//显示站名
function showstationname(showtype) {

    if (points != null) {
        for (var i = 0; i < points.length; i++) {
            var omk = points[i];
            if (showtype == "-1") {
                omk.label.setVisible(false);
            }
            else {
                var Station = $("#cbxStation").attr("checked");
                var Pole = $("#cbxPole").attr("checked");
                if (showtype == "0") {
                    var info = omk.info.content;
                    var lbl = info.substring(info.indexOf('[') + 1, info.indexOf(']'));
                    omk.setLabel(new MLabel(lbl));
                    omk.label.setVisible(true);
                }
                else if (showtype == "1") {
                    var info = omk.info.content;
                    var txt = info.replace(/<\/?.+?>/g, "").replace('资产编号', '*').replace("站点照片", "#");
                    var lbl = txt.substring(txt.indexOf('*') + 1, txt.indexOf('#'));
                    if ((Station && Pole) || (!Station && !Pole)) {
                        omk.setLabel(new MLabel(lbl));
                    }
                    else if (Station) {
                        if (lbl.indexOf('亭') > -1 && lbl.indexOf('牌') > -1) {
                            var infos = lbl.split('）');
                            if (infos[0].indexOf('亭') > -1) {
                                omk.setLabel(new MLabel(infos[0].replace('（亭', "")))
                            }
                            else if (infos[1].indexOf('亭') > -1) {
                                omk.setLabel(new MLabel(infos[1].replace('（亭', "")))
                            }
                        }
                        else {
                            omk.setLabel(new MLabel(lbl.replace('（亭）', "")));
                        }
                    }
                    else if (Pole) {
                        if (lbl.indexOf('亭') > -1 && lbl.indexOf('牌') > -1) {

                            var infos = lbl.split('）');
                            if (infos[0].indexOf('牌') > -1) {
                                omk.setLabel(new MLabel(infos[0].replace('（牌', "")))
                            }
                            else if (infos[1].indexOf('牌') > -1) {
                                omk.setLabel(new MLabel(infos[1].replace('（牌', "")))
                            }
                        }
                        else {
                            omk.setLabel(new MLabel(lbl.replace('（牌）', "")));
                        }
                    }

                    omk.label.setVisible(true);
                }
            }
        }
    }
    maplet.refresh();
}
//准备添加的功能
function startORI() {
    stopORI();
    timerId = window.setInterval(updatePos, 100);
}
function updatePos() {
    if (counter < lines.length - 1) {
        var pointstr = lines[counter];
        var lat = pointstr.split(',')[0];
        var lon = pointstr.split(',')[1];
        var point = new MPoint(lat, lon); //坐标点
        if (counter == 0) {
            chepic = new MMarker(point, new MIcon("/images/icons/RedCar.png", 21, 22));
            maplet.addOverlay(chepic);
        }
        else {
            chepic.setPoint(point); //更新位置
        }
        counter++;

    } else {
        maplet.removeOverlay(chepic, false);
        startORI();
    }
}
function stopORI() {
    if (timerId) window.clearInterval(timerId);
    counter = 0;
}

//线路轨迹回放
function SearchMoreRoadLine() {
    clearMap();
    var RoadLines = $("#txtRoadLines").val();
    if (RoadLines == "") {
        alert("至少选择一条线路！");
        return false;

    }
    //如果查询结果部分是关闭的则在这里打开
    if ($("#left").css("display") != "block") {
        $("#hideResult").click();
    }
    var jsondata = '{RoadLines:"' + RoadLines + '"}';
    var rqurl = '/WebService.asmx/getRoadLineListByRoadLines';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var items = $.parseJSON(callbackdata).d;
    if (items == null) {
        return false;
    }
    removeAllOverlays();
    globaldata = items;
    var tbl_temp = "";
    for (var k = 0; k < items.length; k++) {
        var item = items[k];
        $("#result").hide();
        /*---将得到的地图数据放入表格中---*/
        if (k == 0) {
            tbl_temp += "<div style='cursor:pointer;'><table style='border-style: solid; border-width: 1px;border-color: #CCCCCC #999 #999 #CCCCCC;width:100%;text-align:left' "
                        + "onclick='javascript:{ if ($(\"#result" + k + "\").css(\"display\") == \"block\") {$(\"#result" + k + "\").hide();$(\"#img" + k
                        + "\").attr(\"src\",\"/images/plus.png\");}else{$(\"#result" + k + "\").show();$(\"#img" + k
                        + "\").attr(\"src\",\"/images/minus.png\");}};'><tr style='background-color:" + getRoadLineColor(k) + ";color:White;'><th><img style='border:0px' src='/Resource/Images/plus.png' id='img" + k
                        + "' /></th><td>" + item.RoadLineInfo + "(" + item.TicketStyle + ")</td></tr></table></div><div style='font-size:12px; display:block;' id='result" + k
                        + "'><table style='position:relative;left:10px;font-size:12px;height:120%;top:0px;'  Id=\'TblMap" + k + "\'>";
        }
        else {
            tbl_temp += "<div style='cursor:pointer;'><table style='border-style: solid; border-width: 1px;border-color: #CCCCCC #999 #999 #CCCCCC;width:100%;text-align:left' "
                        + "onclick='javascript:{ if ($(\"#result" + k + "\").css(\"display\") == \"block\") {$(\"#result" + k + "\").hide();$(\"#img" + k
                        + "\").attr(\"src\",\"/images/plus.png\");}else{$(\"#result" + k + "\").show();$(\"#img" + k
                        + "\").attr(\"src\",\"/images/minus.png\");}};'><tr style='background-color:" + getRoadLineColor(k) + ";color:White;'><th><img style='border:0px' src='/Resource/Images/plus.png' id='img" + k
                        + "' /></th><td>" + item.RoadLineInfo + "(" + item.TicketStyle + ")</td></tr></table></div><div style='font-size:12px; display:none;' id='result" + k
                        + "'><table style='position:relative;left: 10px;font-size:12px;height:120%;top:0px;'  Id=\'TblMap" + k + "\'>";
        }
        for (var j = 0; j < item.StationPointInfos.length; j++) {
            var strLatLon = item.StationPointInfos[j].StrLatlon;
            var labl = item.StationPointInfos[j].PointName; //标签点的名称
            var SETime = item.StationPointInfos[j].SETime; //首末班车时间
            var LineListleng = item.StationPointInfos[j].LineListleng;
            if (strLatLon != "") {
                var lat = item.StationPointInfos[j].StrLatlon.split(',')[0]; //经度
                var lon = item.StationPointInfos[j].StrLatlon.split(',')[1]; //纬度
                var point = new MPoint(lat, lon); //坐标
                if (j == 0) {
                    marker = new MMarker(point, new MIcon("/images/icons/起点.png", 16, 16));
                }
                else if (j == item.StationPointInfos.length - 1) {
                    marker = new MMarker(point, new MIcon("/images/icons/终点.png", 16, 16));
                }
                else {
                    marker = new MMarker(point, new MIcon("/images/icons/1/icon_03.png", 8, 8));
                }
                marker.setLabel(new MLabel(labl), true); //添加标签
                marker.label.setVisible(false)//隐藏标签
                ////
                var lineList = item.StationPointInfos[j].LineList.split("、");
                var strlineList = "";
                for (var i = 0; i < lineList.length; i++) {
                    if (strlineList != "") {
                        strlineList += "、<a href='javascript:;' style='text-decoration: none;' onclick='javascript:searchroadline(\"" + lineList[i] + "\",\"" + item.StationPointInfos[j].StopId + "\",\"" + strLatLon + "\")'>" + lineList[i] + "</a>"

                    } else {
                        strlineList += "<a href='javascript:;' style='text-decoration: none;' onclick='javascript:searchroadline(\"" + lineList[i] + "\",\"" + item.StationPointInfos[j].StopId + "\",\"" + strLatLon + "\")'>" + lineList[i] + "</a>"
                    }
                }
                var CaptionNumberInfo = getCaptionNumberInfo(item.StationPointInfos[j]);
                var CaptionNumberIN = 0;
                var Facilitiestxt = "";
                if (item.StationPointInfos[j].FacilitiesSUM != undefined) {
                    var Facs = item.StationPointInfos[j].FacilitiesSUM;
                    $(Facs).each(function (k) {
                        var FacilitiesType = Facs[k].FacilitiesType;
                        var Numbers = Facs[k].Number;
                        if (FacilitiesType == "无电候车亭") {
                            if (Numbers != "0") {
                                Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                                CaptionNumberIN += Number(Numbers);
                            }
                            else {
                                Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                            }
                        }
                        if (FacilitiesType == "有电候车亭") {
                            if (Numbers != "0") {
                                Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                                CaptionNumberIN += Number(Numbers);
                            }
                            else {
                                Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                            }
                        }
                        if (FacilitiesType == "视频") {
                            if (Numbers != "0") {
                                Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                            }
                            else {
                                Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                            }
                        }
                        if (FacilitiesType == "灯片") {
                            if (Numbers != "0") {
                                Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                            }
                            else {
                                Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                            }
                        }
                        if (FacilitiesType == "立杆") {
                            if (Numbers != "0") {
                                Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                                CaptionNumberIN += Number(Numbers);
                            }
                            else {
                                Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                            }
                        }
                    });
                }


                var height = 228;
                //设施数量
                var CaptionNumberManager = 0
                if (CaptionNumberIN > 4) {
                    CaptionNumberManager = 15;
                }
                //站址
                var byteLen = byteLength(item.StationPointInfos[j].Address);
                var byteLenManager = 0;
                if (byteLen > 36) {
                    byteLenManager = 15;
                }

                if (LineListleng <= 35) {
                    height = 150 + CaptionNumberManager + byteLenManager;
                }
                if (LineListleng > 35 && LineListleng <= 68) {
                    height = 168 + CaptionNumberManager + byteLenManager;
                }
                if (LineListleng > 68 && LineListleng <= 102) {
                    height = 185 + CaptionNumberManager + byteLenManager;
                }
                if (LineListleng > 102 && LineListleng <= 125) {
                    height = 201 + CaptionNumberManager + byteLenManager;
                }
               // //height -= 20;
                var str_div = "<div id='RoundedCorner" + j + "' style='height:" + height + "px;width:280px;border-top:1px solid #7b9ebd;background-image:url(/Resource/images/imgs/03.png);border-left:1px solid #008d44;border-right:1px solid #008d44;margin-top:105px; margin-left:1px'>";
                str_div += "<div style=' width:100%; height:30px; z-index:100; background-image:url(/Resource/images/imgs/1.jpg);font-size:12px; line-height:30px; color:#fff; '><span style=' float:left; padding-left:10px'>公交站点：" + labl + "</span><img src='/Resource/Images/imgs/close.jpg' style=' float:right;text-decoration:none;margin-top:5px;cursor:pointer' onclick='colse(RoundedCorner" + j + ")'/></div>";


                str_div += " <div>"
                str_div += "<table style='font-size: 12px'><tr><td style='width:60px'>站点站址</td><td style='width:220px;'>" + item.StationPointInfos[j].Address + "</td></tr><tr><td>停靠线路</td><td style='width:220px;'>" + strlineList + "</td></tr>";
                str_div += "<tr><td>站点编号</td><td><span href='javascript:;' onclick='javascript:linkztsystem(\"" + item.StationPointInfos[j].StopId + "\")'>" + item.StationPointInfos[j].StopId + "</span></td></tr>";
                str_div += "<tr><td>资产编号</td><td style='width:220px;'>" + CaptionNumberInfo + "</td></tr>";
                str_div += "<tr><td>站点照片</td><td><a href='javascript:;' onclick='javascript:openimage(\"" + item.StationPointInfos[j].StopId + "\")'>点击查看照片</a></td></tr>";
                str_div += "</table>";
                str_div += "<div style='display:none;'>[" + labl + "]</div></div>";
                if (Facilitiestxt != "" && Facilitiestxt != null && Facilitiestxt != undefined) {
                    str_div += "<div style='height:20px; font-size: 15px;'>" + Facilitiestxt + "</div>";
                }
                else {
                    str_div += "<div style='height:24px; font-size: 15px;'></div>";
                }
                str_div += "<div style='width:100%; background-image:url(/Resource/images/imgs/0300_02.png); height:69px;overflow:hidden; background-position:-10px' ></div></div>";


                var m_window = new MInfoWindow("11", str_div);
                marker.info = m_window;
                maplet.addOverlay(marker);
                MEvent.addListener(marker, "iw_beforeopen", showCustomIw);



                tbl_temp += "<tr><td style=\'width:12px; \'></td><td style='color:Blue'><a href='javascript:LatLonToCenter(" + strLatLon + ",false);' style='text-decoration: none;'>" + labl + "</a><input type='hidden'  value='" + strLatLon + "'></td><td >" + SETime + ".</td></tr>";

                MEvent.addListener(marker, "click", function (omarker) {
                    omarker.openInfoWindow();
                });
                MEvent.addListener(marker, "mouseover", function (mk) {
                    if (mk.label) mk.label.setVisible(true);
                })
                MEvent.addListener(marker, "mouseout", function (mk) {
                    if (mk.label) mk.label.setVisible(false);
                })
                points.push(marker);
                maplet.addOverlay(marker);
            }
            else {

                tbl_temp += "<tr><td style=\'width:12px;\'></td><td style='color:Green'>" + labl + "</td><td >" + SETime + ".</td></tr>";
            }
        }
        tbl_temp += "</table></div>";
        //用于存放当前点的集合的数组
        var strlinelatlon = "";
        if (item.LineLatLon != undefined) {
            var points_temp = new Array();
            for (var i = 0; i < item.LineLatLon.length; i++) {

                if (item.LineLatLon[i] != null && item.LineLatLon[i] != "" && item.LineLatLon != undefined) {
                    if (i > 0) {
                        strlinelatlon += ";" + item.LineLatLon[i];
                    }
                    else {
                        strlinelatlon += item.LineLatLon[i];
                    }
                    var lat = item.LineLatLon[i].split(',')[0];
                    var lon = item.LineLatLon[i].split(',')[1];
                    var point = new MPoint(lat, lon);
                    points_temp.push(point);
                }
            }
            //将地图移动到中心位置
            var lat = item.CenterPoint.split(',')[0];
            var lon = item.CenterPoint.split(',')[1]
            maplet.centerAndZoom(new MPoint(lat, lon), 10);
            $("#linecenter").attr("value", item.CenterPoint);
            //创建笔刷将所有的点连接起来
            var brush = new MBrush();
            brush.arrow = false;
            brush.stroke = 5;
            brush.color = getRoadLineColor(k);
            var polyline = new MPolyline(points_temp, brush);
            maplet.addOverlay(polyline);

        }
        $("#roadlines").val(strlinelatlon);
    }
    $("#resultcontent").html(tbl_temp);
    //显示 站名
    showstationname(-1);
}
//
function searchroadline(RoadLine, StopId, strlatlon) {
    //如果查询结果部分是关闭的则在这里打开
    if ($("#left").css("display") != "block") {
        $("#hideResult").click();
    }
    var TotalDiv = "";
    var jsondata = '{RoadLine:"' + RoadLine + '",StopId:"' + StopId + '"}';
    var rqurl = '/WebService.asmx/getRoadLineListByRoadLine';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var Jsonstr = eval('(' + callbackdata + ')');
    var items = Jsonstr.d;
    if (items == null) {
        alert('当前路线不存在！');
        return false;
    }
    points = new Array();
    globaldata = items;
    var tbl_temp = "";
    if (items.length > 0) {
        removeAllOverlays();
    }
    for (var k = 0; k < items.length; k++) {
        var item = items[k];
        if (k == 0) {
            tbl_temp += "<div style='cursor:pointer;'><table style='border-style: solid; border-width: 1px;border-color: #CCCCCC #999 #999 #CCCCCC;width:100%;text-align:left' "
                        + "onclick='javascript:{ if ($(\"#result" + k + "\").css(\"display\") == \"block\") {$(\"#result" + k + "\").hide();$(\"#img" + k
                        + "\").attr(\"src\",\"/images/plus.png\")}else{$(\"#result" + k + "\").show();$(\"#img" + k
                        + "\").attr(\"src\",\"/images/minus.png\");}};'><tr><th><img style='border:0px' src='/Resource/Images/plus.png' id='img" + k
                        + "' /></th><td>" + item.RoadLineInfo + "(" + item.TicketStyle + ")</td></tr></table></div><div style='font-size:12px; display:block;' id='result" + k
                        + "'><table style='font-size:12px;height:100%' Id='TblMap'>";

        }
        else {
            tbl_temp += "<div style='cursor:pointer;'><table style='border-style: solid; border-width: 1px;border-color: #CCCCCC #999 #999 #CCCCCC;width:100%;text-align:left' "
                        + "onclick='javascript:{ if ($(\"#result" + k + "\").css(\"display\") == \"block\") {$(\"#result" + k + "\").hide();$(\"#img" + k
                        + "\").attr(\"src\",\"/images/plus.png\");}else{$(\"#result" + k + "\").show();$(\"#img" + k
                        + "\").attr(\"src\",\"/images/minus.png\");}};'><tr><th><img style='border:0px' src='/Resource/Images/plus.png' id='img" + k
                        + "' /></th><td>" + item.RoadLineInfo + "(" + item.TicketStyle + ")</td></tr></table></div><div style='font-size:12px; display:none;' id='result" + k
                        + "'><table style='font-size:12px;height:100%' Id='TblMap'>";
        }
        if (item.StationPointInfos != undefined) {
            $(item.StationPointInfos).each(function (j) {
                var StationPoint = item.StationPointInfos[j];
                var strLatLon = item.StationPointInfos[j].StrLatlon;
                var labl = StationPoint.PointName; //标签点的名称
                var SETime = StationPoint.SETime; //首末班车时间
                var LineListleng = StationPoint.LineListleng;
                if (strLatLon != "") {
                    var lat = StationPoint.StrLatlon.split(',')[0]; //经度
                    var lon = StationPoint.StrLatlon.split(',')[1]; //纬度
                    if (j == item.StationPointInfos.length - 1 && k == items.length - 1) {
                        var RoadNum = item.RoadNum; //路线总数
                        var BrandNum = item.BrandNum; //站牌总数
                        TotalDiv = "<div><table style='height:40px; float: left; margin-left: 8px;'><td>路线数：" + RoadNum + "</td><td> &nbsp;&nbsp;&nbsp;&nbsp;站牌数：" + BrandNum + "</td></table></div>";
                    }
                    var point = new MPoint(lat, lon); //坐标
                    if (j == 0) {
                        marker = new MMarker(point, new MIcon("/images/icons/起点.png", 16, 16));
                    }
                    else if (j == (item.StationPointInfos.length - 1)) {
                        marker = new MMarker(point, new MIcon("/images/icons/终点.png", 16, 16));
                    }
                    else {

                        if (strlatlon == strLatLon) {
                            marker = new MMarker(point, new MIcon("/images/icons/小蓝.png", 20, 20));
                        }
                        else {
                            marker = new MMarker(point, new MIcon("/images/icons/1/icon_03.png", 8, 8));
                        }
                    }
                    marker.setLabel(new MLabel(labl), true); //添加标签
                    marker.label.setVisible(false)//隐藏标签
                    ////
                    var lineList = StationPoint.LineList.split("、");
                    var strlineList = "";
                    for (var i = 0; i < lineList.length; i++) {
                        if (strlineList != "") {
                            strlineList += "、<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[i] + "\",\"" + item.StationPointInfos[j].StopId + "\",\"" + strLatLon + "\")'>" + lineList[i] + "</a>"
                        } else {
                            strlineList += "<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[i] + "\",\"" + item.StationPointInfos[j].StopId + "\",\"" + strLatLon + "\")'>" + lineList[i] + "</a>"
                        }
                    }
                    var CaptionNumberInfo = getCaptionNumberInfo(StationPoint);
                    var CaptionNumberIN = 0;
                    var Facilitiestxt = "";
                    if (item.StationPointInfos[j].FacilitiesSUM != undefined) {
                        var Facs = item.StationPointInfos[j].FacilitiesSUM;
                        $(Facs).each(function (k) {
                            var FacilitiesType = Facs[k].FacilitiesType;
                            var Numbers = Facs[k].Number;
                            if (FacilitiesType == "无电候车亭") {
                                if (Numbers != "0") {
                                    Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                                    CaptionNumberIN += Number(Numbers);
                                }
                                else {
                                    Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                                }
                            }
                            if (FacilitiesType == "有电候车亭") {
                                if (Numbers != "0") {
                                    Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                                    CaptionNumberIN += Number(Numbers);
                                }
                                else {
                                    Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                                }
                            }
                            if (FacilitiesType == "视频") {
                                if (Numbers != "0") {
                                    Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                                }
                                else {
                                    Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                                }
                            }
                            if (FacilitiesType == "灯片") {
                                if (Numbers != "0") {
                                    Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                                }
                                else {
                                    Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                                }
                            }
                            if (FacilitiesType == "立杆") {
                                if (Numbers != "0") {
                                    Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                                    CaptionNumberIN += Number(Numbers);
                                }
                                else {
                                    Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                                }
                            }
                        });
                    }

                    var height = 228;
                    //设施数量
                    var CaptionNumberManager = 0
                    if (CaptionNumberIN > 4) {
                        CaptionNumberManager = 15;
                    }
                    //站址
                    var byteLen = byteLength(item.StationPointInfos[j].Address);
                    var byteLenManager = 0;
                    if (byteLen > 36) {
                        byteLenManager = 15;
                    }

                    if (LineListleng <= 35) {
                        height = 150 + CaptionNumberManager + byteLenManager;
                    }
                    if (LineListleng > 35 && LineListleng <= 68) {
                        height = 168 + CaptionNumberManager + byteLenManager;
                    }
                    if (LineListleng > 68 && LineListleng <= 102) {
                        height = 185 + CaptionNumberManager + byteLenManager;
                    }
                    if (LineListleng > 102 && LineListleng <= 125) {
                        height = 201 + CaptionNumberManager + byteLenManager;
                    }
                    //height -= 20;

                    var str_div = "<div id='RoundedCorner" + j + "' style='height:" + height + "px;width:280px;border-top:1px solid #7b9ebd;background-image:url(/Resource/images/imgs/03.png);border-left:1px solid #008d44;border-right:1px solid #008d44;margin-top:105px; margin-left:1px'>";
                    str_div += "<div style=' width:100%; height:30px; z-index:100; background-image:url(/Resource/images/imgs/1.jpg);font-size:12px; line-height:30px; color:#fff; '><span style=' float:left; padding-left:10px'>公交站点：" + labl + "</span><img src='/Resource/Images/imgs/close.jpg' style=' float:right;text-decoration:none;margin-top:5px;cursor:pointer' onclick='colse(RoundedCorner" + j + ")'/></div>";
                    str_div += " <div>"
                    str_div += "<table style='font-size: 12px'><tr><td style='width:60px'>站点站址</td><td style='width:220px;'>" + item.StationPointInfos[j].Address + "</td></tr><tr><td>停靠线路</td><td style='width:220px;'>" + strlineList + "</td></tr>";
                    str_div += "<tr><td>站点编号</td><td><span href='javascript:;' onclick='javascript:linkztsystem(\"" + item.StationPointInfos[j].StopId + "\")'>" + item.StationPointInfos[j].StopId + "</span></td></tr>";
                    str_div += "<tr><td>资产编号</td><td style='width:220px;'>" + CaptionNumberInfo + "</td></tr>";
                     str_div += "<tr><td>站点照片</td><td><a href='javascript:;' onclick='javascript:openimage(\"" + item.StationPointInfos[j].StopId + "\")'>点击查看照片</a></td></tr>";
                    str_div += "</table>";

                    str_div += "<div style='display:none;'>[" + labl + "]</div></div>";
                    if (Facilitiestxt != "" && Facilitiestxt != null && Facilitiestxt != undefined) {
                        str_div += "<div style='height:20px; font-size: 15px;'>" + Facilitiestxt + "</div>";
                    }
                    else {
                        str_div += "<div style='height:24px; font-size: 15px;'></div>";
                    }
                    str_div += "<div style='width:100%; background-image:url(/Resource/images/imgs/0300_02.png); height:69px;overflow:hidden; background-position:-10px' ></div></div>";

                    var m_window = new MInfoWindow("11", str_div);
                    marker.info = m_window;
                    maplet.addOverlay(marker);
                    MEvent.addListener(marker, "iw_beforeopen", showCustomIw);


                    if (j == 0) {
                        tbl_temp += "<tr><td>站级</td><td>站名</td><td>首末班时间</td></tr>";
                        tbl_temp += "<tr><td>" + (j + 1) + ".</td><td style='color:Blue'><a href='javascript:LatLonToCenter(" + strLatLon + ",true)'>" + labl + "</a><input type='hidden'  value='" + strLatLon + "'></td><td >" + SETime + ".</td></tr>";
                    }
                    else {
                        tbl_temp += "<tr><td>" + (j + 1) + ".</td><td style='color:Blue;'><a href='javascript:LatLonToCenter(" + strLatLon + ",true)'>" + labl + "</a><input type='hidden'  value='" + strLatLon + "'></td><td >" + SETime + ".</td></tr>";
                    }
                    MEvent.addListener(marker, "click", function (omarker) {
                        omarker.openInfoWindow();
                    });
                    MEvent.addListener(marker, "mouseover", function (mk) {
                        if (mk.label) mk.label.setVisible(true);
                    })
                    MEvent.addListener(marker, "mouseout", function (mk) {
                        if (mk.label) mk.label.setVisible(false);
                    })
                    points.push(marker);
                    maplet.addOverlay(marker);
                }
                else {
                    if (j == 0) {
                        tbl_temp += "<tr><td>站级</td><td>站名</td><td>首末班时间</td></tr>";
                        tbl_temp += "<tr><td>" + (j + 1) + ".</td><td style='color:Green'>" + labl + "</td><td >" + SETime + ".</td></tr>";
                    }
                    else {
                        tbl_temp += "<tr><td>" + (j + 1) + ".</td><td style='color:Green;'>" + labl + "</td><td >" + SETime + ".</td></tr>";
                    }
                }

            });

        }
        tbl_temp += "</table></div>";
        tbl_temp = TotalDiv + tbl_temp;
        // $("#resultcontent").html(tbl_temp);
        //将所有经纬度添加到集合中去
        //用于存放当前点的集合的数组
        var strlinelatlon = "";
        var points_temp = new Array();
        for (var i = 0; i < item.LineLatLon.length; i++) {

            if (item.LineLatLon[i] != null && item.LineLatLon[i] != "" && item.LineLatLon != undefined) {
                if (i > 0) {
                    strlinelatlon += ";" + item.LineLatLon[i];
                }
                else {
                    strlinelatlon += item.LineLatLon[i];
                }
                var lat = item.LineLatLon[i].split(',')[0];
                var lon = item.LineLatLon[i].split(',')[1];
                var point = new MPoint(lat, lon);
                points_temp.push(point);
            }
        }
        //将地图移动到中心位置
        var lat = item.CenterPoint.split(',')[0];
        var lon = item.CenterPoint.split(',')[1]
        maplet.centerAndZoom(new MPoint(lat, lon), 10);
        $("#linecenter").attr("value", item.CenterPoint);
        //创建笔刷将所有的点连接起来
        var brush = new MBrush();
        brush.arrow = false;
        brush.stroke = 4;
        brush.fill = false;
        brush.color = getRoadLineColor(k);
        brush.bgcolor = 'red';
        var polyline = new MPolyline(points_temp, brush);
        maplet.addOverlay(polyline);
        $("#roadlines").val(strlinelatlon);
    }
    if (tbl_temp == "") {
        window.alert("当前线路未确认！");
        return false;
    }
    $("#resultcontent").html(tbl_temp);
    //显示 站名
    showstationname(-1);
}
function DrawingLines(ItemId) {
    points = new Array();
    removeAllOverlays();
    var item = globaldata[ItemId];
    for (var j = 0; j < item.StationPointInfos.length; j++) {
        var strLatLon = item.StationPointInfos[j].StrLatlon;
        var LineListleng = item.StationPointInfos[j].LineListleng;
        if (strLatLon == "") {
            continue;
        }
        var lat = item.StationPointInfos[j].StrLatlon.split(',')[0]; //经度
        var lon = item.StationPointInfos[j].StrLatlon.split(',')[1]; //纬度
        var labl = item.StationPointInfos[j].PointName; //标签点的名称
        var SETime = item.StationPointInfos[j].SETime; //首末班车时间
        var point = new MPoint(lat, lon); //坐标
        if (j == 0) {
            marker = new MMarker(point, new MIcon("/images/icons/起点.png", 16, 16));
        }
        else if (j == (item.StationPointInfos.length - 1)) {
            marker = new MMarker(point, new MIcon("/images/icons/终点.png", 16, 16));
        }
        else {
            marker = new MMarker(point, new MIcon("/images/icons/1/icon_03.png", 8, 8));
        }
        marker.setLabel(new MLabel(labl), true); //添加标签
        marker.label.setVisible(false)//隐藏标签
        ////
        var lineList = item.StationPointInfos[j].LineList.split("、");
        var strlineList = "";
        for (var i = 0; i < lineList.length; i++) {
            if (strlineList != "") {
                strlineList += "、<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[i] + "\",\"" + item.StationPointInfos[j].StopId + "\",\"" + strLatLon + "\")'>" + lineList[i] + "</a>"
            } else {
                strlineList += "<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[i] + "\",\"" + item.StationPointInfos[j].StopId + "\",\"" + strLatLon + "\")'>" + lineList[i] + "</a>"
            }
        }
        //  + "<tr><td>资产编号</td><td>" + CaptionNumberInfo + "</td></tr>"
        var CaptionNumberInfo = getCaptionNumberInfo(item.StationPointInfos[j]);
        var CaptionNumberIN = 0;
        var Facilitiestxt = "";
        if (item.StationPointInfos[j].FacilitiesSUM != undefined) {
            var Facs = item.StationPointInfos[j].FacilitiesSUM;
            $(Facs).each(function (k) {
                var FacilitiesType = Facs[k].FacilitiesType;
                var Numbers = Facs[k].Number;
                if (FacilitiesType == "无电候车亭") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        CaptionNumberIN += Number(Numbers);
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "有电候车亭") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        CaptionNumberIN += Number(Numbers);
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "视频") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "灯片") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "立杆") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        CaptionNumberIN += Number(Numbers);
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
            });
        }

        var height = 228;
        //设施数量
        var CaptionNumberManager = 0
        if (CaptionNumberIN > 4) {
            CaptionNumberManager = 15;
        }
        //站址
        var byteLen = byteLength(item.StationPointInfos[j].Address);
        var byteLenManager = 0;
        if (byteLen > 36) {
            byteLenManager = 15;
        }

        if (LineListleng <= 35) {
            height = 150 + CaptionNumberManager + byteLenManager;
        }
        if (LineListleng > 35 && LineListleng <= 68) {
            height = 168 + CaptionNumberManager + byteLenManager;
        }
        if (LineListleng > 68 && LineListleng <= 102) {
            height = 185 + CaptionNumberManager + byteLenManager;
        }
        if (LineListleng > 102 && LineListleng <= 125) {
            height = 201 + CaptionNumberManager + byteLenManager;
        }
        //height -= 20;
        var str_div = "<div id='RoundedCorner" + j + "' style='height:" + height + "px;width:280px;border-top:1px solid #7b9ebd;background-image:url(/Resource/images/imgs/03.png);border-left:1px solid #008d44;border-right:1px solid #008d44;margin-top:105px; margin-left:1px'>";
        str_div += "<div style=' width:100%; height:30px; z-index:100; background-image:url(/Resource/images/imgs/1.jpg);font-size:12px; line-height:30px; color:#fff; '><span style=' float:left; padding-left:10px'>公交站点：" + labl + "</span><img src='/Resource/Images/imgs/close.jpg' style=' float:right;text-decoration:none;margin-top:5px;cursor:pointer' onclick='colse(RoundedCorner" + j + ")'/></div>";


        str_div += " <div>"
        str_div += "<table style='font-size: 12px'><tr><td style='width:60px'>站点站址</td><td style='width:220px;'>" + item.StationPointInfos[j].Address + "</td></tr><tr><td>停靠线路</td><td style='width:220px;'>" + strlineList + "</td></tr>";
        str_div += "<tr><td>站点编号</td><td><span href='javascript:;' onclick='javascript:linkztsystem(\"" + item.StationPointInfos[j].StopId + "\")'>" + item.StationPointInfos[j].StopId + "</span></td></tr>";
        str_div += "<tr><td>资产编号</td><td style='width:220px;' >" + CaptionNumberInfo + "</td></tr>";
        str_div += "<tr><td>站点照片</td><td><a href='javascript:;' onclick='javascript:openimage(\"" + item.StationPointInfos[j].StopId + "\")'>点击查看照片</a></td></tr>";
        str_div += "</table>";
        str_div += "<div style='display:none;'>[" + labl + "]</div></div>";
        if (Facilitiestxt != "" && Facilitiestxt != null && Facilitiestxt != undefined) {
            str_div += "<div style='height:20px; font-size: 15px;'>" + Facilitiestxt + "</div>";
        }
        else {
            str_div += "<div style='height:24px; font-size: 15px;'></div>";
        }

        str_div += "<div style='width:100%; background-image:url(/Resource/images/imgs/0300_02.png); height:69px;overflow:hidden; background-position:-10px' ></div></div>";

        var m_window = new MInfoWindow("11", str_div);
        marker.info = m_window;
        maplet.addOverlay(marker);
        MEvent.addListener(marker, "iw_beforeopen", showCustomIw);


        // 添加对marker 的事件
        MEvent.addListener(marker, "click", function (mk) {
            mk.openInfoWindow();
        });
        MEvent.addListener(marker, "mouseover", function (mk) {
            if (mk.label) mk.label.setVisible(true);
        });
        MEvent.addListener(marker, "mouseout", function (mk) {
            if (mk.label) mk.label.setVisible(false);
        });
        points.push(marker);
        maplet.addOverlay(marker);
    }
    var strlinelatlon = "";
    var points_temp = new Array();
    if (item.LineLatLon != null) {
        for (var i = 0; i < item.LineLatLon.length; i++) {

            if (item.LineLatLon[i] != null && item.LineLatLon[i] != "" && item.LineLatLon != undefined) {
                if (i > 0) {
                    strlinelatlon += ";" + item.LineLatLon[i];
                }
                else {
                    strlinelatlon += item.LineLatLon[i];
                }
                var lat = item.LineLatLon[i].split(',')[0];
                var lon = item.LineLatLon[i].split(',')[1];
                var point = new MPoint(lat, lon);
                points_temp.push(point);
            }
        }
        //将地图移动到中心位置
        var lat = item.CenterPoint.split(',')[0];
        var lon = item.CenterPoint.split(',')[1]
        maplet.centerAndZoom(new MPoint(lat, lon), 10);
        $("#linecenter").attr("value", item.CenterPoint);
        //创建笔刷将所有的点连接起来
        var brush = new MBrush();
        brush.arrow = false;
        brush.stroke = 4;
        brush.fill = false;
        brush.color = getRoadLineColor(ItemId);
        brush.bgcolor = 'red';
        var polyline = new MPolyline(points_temp, brush);
        maplet.addOverlay(polyline);
    }
    $("#roadlines").val(strlinelatlon);
}
//查询多条路段上面的站点和经过线路
function SearchRoadsPoint() {
    clearMap();
    if ($("#left").css("display") != "block") {
        $("#hideResult").click();
    }
    var txtRoads = $("#txtRoads").val();
    if (txtRoads == "") {
        alert("请选择一条线路！")
    }
    else {
        var Roads = txtRoads.split(',');
        var jsondata = '{Roads:"' + Roads + '"}';
        var rqurl = '/WebService.asmx/getRoadLineListByRoadName';
        Jquery_SyncAjax(jsondata, rqurl, true, LoadRoadsPoint);
    }
}
//加载经过线路信息系
function LoadRoadsPoint(data, textStatus) {
    var odata = $.parseJSON($.parseJSON(data).d);
    if (odata == null) {
        return false;
    }
    removeAllOverlays();
    points = new Array();
    var stopcount = 0;
    var roadlinecount = 0;
    /*---请求地图数据end ---*/
    /*---将得到的地图数据放入表格中---*/
    var addroads = new Array();

    stopcount = odata.PointNumber;
    roadlinecount = odata.RoadNumber
    var TotalDiv = "<div style='height:30px;top;20px; margin-left: 8px;'><table ><tr><td><div id='divtotal'></div></td></tr></table></div>"; ;
    var tbl_temp = TotalDiv + "<div style='font-size:12px;'><table  Id='TblMap'>"
    for (var i = 0; i < odata.Points.length; i++) {
        var strLatLon = odata.Points[i].Strlatlon;
        if (strLatLon != null && strLatLon != "") {
            var lat = strLatLon.split(',')[0]; //经度
            var lon = strLatLon.split(',')[1]; //纬度
            var labl = odata.Points[i].StationName; //标签点的名称
            var point = new MPoint(lat, lon); //坐标点
            var LineListleng = odata.Points[i].LineListleng; 
            marker = new MMarker(point, new MIcon("/images/icons/小红.png", 21, 22));
            var new_shado = new MIconShadow("/images/icons/shadow.png", 27, 34, -14)
            marker.setShadow(new_shado, true)

            var lineList = odata.Points[i].LineList.split("、");
            var strlineList = "";
            for (var j = 0; j < lineList.length; j++) {
                if (strlineList != "") {
                    strlineList += "、<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[j] + "\",\"" + odata.Points[i].StopId + "\",\"" + strLatLon + "\")'>" + lineList[j] + "</a>"

                } else {
                    strlineList += "<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[j] + "\",\"" + odata.Points[i].StopId + "\",\"" + strLatLon + "\")'>" + lineList[j] + "</a>"
                }
            }
            var CaptionNumberInfo = getCaptionNumberInfo(odata.Points[i]);
            var CaptionNumberIN = 0;

            var Facilitiestxt = "";
            if (odata.Points[i].FacilitiesSUM != undefined) {
                var Facs = odata.Points[i].FacilitiesSUM;
                $(Facs).each(function (k) {
                    var FacilitiesType = Facs[k].FacilitiesType;
                    var Numbers = Facs[k].Number;
                    if (FacilitiesType == "无电候车亭") {
                        if (Numbers != "0") {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                            CaptionNumberIN += Number(Numbers);
                        }
                        else {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                        }
                    }
                    if (FacilitiesType == "有电候车亭") {
                        if (Numbers != "0") {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                            CaptionNumberIN += Number(Numbers);
                        }
                        else {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                        }
                    }
                    if (FacilitiesType == "视频") {
                        if (Numbers != "0") {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        }
                        else {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                        }
                    }
                    if (FacilitiesType == "灯片") {
                        if (Numbers != "0") {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        }
                        else {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                        }
                    }
                    if (FacilitiesType == "立杆") {
                        if (Numbers != "0") {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                            CaptionNumberIN += Number(Numbers);
                        }
                        else {
                            Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                        }
                    }
                });
            }

            var height = 228;
            //设施数量
            var CaptionNumberManager = 0
            if (CaptionNumberIN > 4) {
                CaptionNumberManager = 15;
            }
            //站址
            var byteLen = byteLength(odata.Points[i].StationAddress);
            var byteLenManager = 0;
            if (byteLen > 36) {
                byteLenManager = 15;
            }

            if (LineListleng <= 35) {
                height = 150 + CaptionNumberManager + byteLenManager;
            }
            if (LineListleng > 35 && LineListleng <= 68) {
                height = 168 + CaptionNumberManager + byteLenManager;
            }
            if (LineListleng > 68 && LineListleng <= 102) {
                height = 185 + CaptionNumberManager + byteLenManager;
            }
            if (LineListleng > 102 && LineListleng <= 125) {
                height = 201 + CaptionNumberManager + byteLenManager;
            }
            //height -= 20;
            var str_div = "<div id='RoundedCorner" + i + "' style='height:" + height + "px;width:280px;border-top:1px solid #7b9ebd;background-image:url(/Resource/images/imgs/03.png);border-left:1px solid #008d44;border-right:1px solid #008d44;margin-top:105px; margin-left:1px'>";
            str_div += "<div style=' width:100%; height:30px; z-index:100; background-image:url(/Resource/images/imgs/1.jpg);font-size:12px; line-height:30px; color:#fff; '><span style=' float:left; padding-left:10px'>公交站点：" + labl + "</span><img src='/Resource/Images/imgs/close.jpg' style=' float:right;text-decoration:none;margin-top:5px;cursor:pointer' onclick='colse(RoundedCorner" + i + ")'/></div>";

            str_div += " <div>"
            str_div += "<table style='font-size: 12px'><tr><td style='width:60px'>站点站址</td><td style='width:220px;'>" + odata.Points[i].StationAddress + "</td></tr><tr><td>停靠线路</td><td style='width:220px;'>" + strlineList + "</td></tr>";
            str_div += "<tr><td>站点编号</td><td><a href='javascript:;' onclick='javascript:linkztsystem(\"" + odata.Points[i].StopId + "\")'>" + odata.Points[i].StopId + "</a></td></tr>";
            str_div += "<tr><td>资产编号</td><td style='width:220px;'>" + CaptionNumberInfo + "</td></tr>";
            str_div += "<tr><td>站点照片</td><td><a href='javascript:;' onclick='javascript:openimage(\"" + odata.Points[i].StopId + "\")'>点击查看照片</a></td></tr>";
            str_div += "</table>";
            str_div += "<div style='display:none;'>[" + labl + "]</div></div>";
            if (Facilitiestxt != "" && Facilitiestxt != null && Facilitiestxt != undefined) {
                str_div += "<div style='height:20px; font-size: 15px;'>" + Facilitiestxt + "</div>";
            }
            else {
                str_div += "<div style='height:24px; font-size: 15px;'></div>";
            }

            str_div += "<div style='width:100%; background-image:url(/Resource/images/imgs/0300_02.png); height:69px;overflow:hidden; background-position:-10px' ></div></div>";
           

            var m_window = new MInfoWindow("11", str_div);
            marker.info = m_window;
            marker.setLabel(new MLabel(labl), true); //添加标签
            marker.label.setVisible(false)//隐藏标签

            maplet.addOverlay(marker);
            MEvent.addListener(marker, "iw_beforeopen", showCustomIw);

            //marker.bEditable = true;
            // 添加对marker 的事件
            MEvent.addListener(marker, "click", function (omarker) {
                omarker.openInfoWindow();
            });
            MEvent.addListener(marker, "mouseover", function (mk) {
                if (mk.label) mk.label.setVisible(true);
            })
            MEvent.addListener(marker, "mouseout", function (mk) {
                if (mk.label) mk.label.setVisible(false);
            })
            points.push(marker);
            maplet.addOverlay(marker);
            //将地图移动到中心位置
            if (i == parseInt(odata.length / 2)) {
                maplet.centerAndZoom(point, 10);
            }
            tbl_temp += "<tr><td>" + (i + 1) + ".<a href='javascript:LatLonToCenter(" + strLatLon + ",false)'>" + odata.Points[i].StationName + odata.Points[i].StationAddress + "</a></td></tr>"
        }
        else {
            tbl_temp += "<tr><td style='color:Green'>" + (i + 1) + "." + odata.Points[i].StationName + odata.Points[i].StationAddress + "</td></tr>"
        }
        if (odata.Points[i].RelationsRoad.length > 0) {
            rqurl = '/WebService.asmx/getRoadLineInfoByRoadLine';
            //加载关联的线路信息 太多数据可能会照成浏览器无法响应
            for (var j = 0; j < odata.Points[i].RelationsRoad.length; j++) {
                var RoadLineInfos = odata.Points[i].RelationsRoad[j].split('-');
                if ($.inArray(odata.Points[i].RelationsRoad[j], addroads) == -1) {
                    addroads.push(odata.Points[i].RelationsRoad[j]);
                    var jsondata = '{RoadLine:"' + RoadLineInfos[0] + '",ToDirection:"' + RoadLineInfos[1] + '"}';
                    Jquery_SyncAjax(jsondata, rqurl, true, LoadRelationsRoadLine)
                }
            }
        }
    }
    tbl_temp += "</table></div>";
    $("#resultcontent").html(tbl_temp);
    $("#divtotal").html("站点数：" + stopcount + "经过线路数：" + roadlinecount);
    //显示 站名
    showstationname(-1); ;

}
//
function SystemLinkSearch() {
    //管理系统连接过来的参数
    if (window.location.search != null && window.location.search != "") {
        var isroad = false;
        var roadline;
        var pangename = "";
        var searchprams = window.location.search.replace("?", "").split("&");
        var Area="";
        if (searchprams[0] == "uin=guest") {
            Area = searchprams[5];
        } else {
            Area = searchprams[6];
         }
        var areas = Area.split("=");
        var jsondata = "{";
        for (var i = 0; i < searchprams.length; i++) {
            var keyname = searchprams[i].split("=")[0];
            var keyvalue = searchprams[i].split("=")[1];
            if (keyname == "uin" || keyname == "skey" || keyname == "token" || keyname == "redirecturl" || keyname == "pagename") {
                if (keyname == "pagename") {
                    if (keyvalue == "roadstation") {
                        isroad = true;
                    }
                }
                if (!isroad && keyname == "pagename") {
                    pangename = keyvalue;
                }

                continue;
            }
            if (keyname == "District") {
                switch (keyvalue) {
                    case "1":
                        keyvalue = "黄浦区";
                        break;
                    case "2":
                        keyvalue = "静安区";
                        break;
                    case "3":
                        keyvalue = "徐汇区";
                        break;
                    case "4":
                        keyvalue = "长宁区";
                        break;
                    case "5":
                        keyvalue = "普陀区";
                        break;
                    case "6":
                        keyvalue = "闸北区";
                        break;
                    case "7":
                        keyvalue = "虹口区";
                        break;
                    case "8":
                        keyvalue = "杨浦区";
                        break;
                    case "9":
                        keyvalue = "闵行区";
                        break;
                    case "10":
                        keyvalue = "宝山区";
                        break;
                    case "11":
                        keyvalue = "嘉定区";
                        break;
                    case "12":
                        keyvalue = "松江区";
                        break;
                    case "13":
                        keyvalue = "青浦区";
                        break;
                    case "14":
                        keyvalue = "奉贤区";
                        break;
                    case "15":
                        keyvalue = "浦东新区";
                        break;
                    case "16":
                        keyvalue = "金山区";
                        break;
                    case "17":
                        keyvalue = "崇明县";
                        break;
                }
            }
            if (jsondata != "{") {
                jsondata += "," + keyname + ":\'" + keyvalue + "\'";
            }
            else {
                jsondata += keyname + ":\'" + keyvalue + "\'";
            }
            if (keyname == "RoadLine") {
                roadline = keyvalue;
            }
        }

        if (pangename != "") {
            jsondata += ",pagename:\'" + pangename + "\'}";
        }
        else {
            jsondata += ",pagename:\'\'}";
        }
        if (isroad) {
            $("#txtRoadLines").val( roadline);
            SearchMoreRoadLine()

        }
        else {
            //{RoadLine:'',Area:'bs001',pagename:'roadstations'}
            if (jsondata == "{RoadLine:'',Area:\'" + areas[1] + "\',pagename:'roadstations'}") {
                linksear(jsondata)
                
            } else {
                linksearchpoint(jsondata)
            }
        }

    }
}
//地图定位站点
function linksear(jsondata) {
    //alert(jsondata);
    var rqurl = '/WebService.asmx/GetPointsByLink';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var Jsonstr = eval('(' + callbackdata + ')');
    var item = eval('(' + Jsonstr.d + ')');
    removeAllOverlays();
    points = new Array();
    /*---请求地图数据end ---*/
    /*---将得到的地图数据放入表格中---*/
    var TotalDiv = "<div style='height:30px;top;20px; margin-left: 8px;'><table ><tr><td>站点数：" + item.length + "</td></tr></table></div>"; ;

    var tbl_temp = TotalDiv + "<div style='font-size:12px;'><table  Id='TblMap'>"
    for (var i = 0; i < item.length; i++) {
        var strLatLon = item[i].Strlatlon;
        if (strLatLon == "") {
            continue;
        }
        var lat = strLatLon.split(',')[0]; //经度
        var lon = strLatLon.split(',')[1]; //纬度
        var labl = item[i].StationName; //标签点的名称
        var point = new MPoint(lat, lon); //坐标点
        var PathDirection = item[i].PathDirection
        var LineListleng = item[i].LineListleng 
        var w = 16;
        var h = 16;
        var icourl = "/images/icons/1/icon_03.png";
        switch (PathDirection) {
            case "枢纽":
                icourl = "/images/icons/1/icon_01.png";
                w = 40;
                h = 32;
                break;
            case "枢纽站":
                icourl = "/images/icons/1/icon_01.png";
                w = 40;
                h = 32;
                break;
            case "终点":
                w = 24;
                h = 24;
                icourl = "/images/icons/1/icon_02.png";
                break;
            case "终点站":
                w = 24;
                h = 24;
                icourl = "/images/icons/1/icon_02.png";
                break;
            default:
                icourl = "/images/icons/1/icon_03.png";
                break;
        }
        marker = new MMarker(point, new MIcon(icourl, w, h));
        var lineList = item[i].LineList.split("、");
        var strlineList = "";
        for (var j = 0; j < lineList.length; j++) {
            if (strlineList != "") {
                strlineList += "、<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[j] + "\",\"" + item[i].StopId + "\",\"" + strLatLon + "\")'>" + lineList[j] + "</a>"

            } else {
                strlineList += "<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[j] + "\",\"" + item[i].StopId + "\",\"" + strLatLon + "\")'>" + lineList[j] + "</a>"
            }
        }
        var CaptionNumberInfo = getCaptionNumberInfo(item[i]);
        var CaptionNumberIN = 0;
        var Facilitiestxt = "";
        if (item[i].FacilitiesSUM != undefined) {
            var Facs = item[i].FacilitiesSUM;
            $(Facs).each(function (k) {
                var FacilitiesType = Facs[k].FacilitiesType;
                var Numbers = Facs[k].Number;
                if (FacilitiesType == "无电候车亭") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        CaptionNumberIN += Number(Numbers);
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "有电候车亭") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        CaptionNumberIN += Number(Numbers);
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "视频") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "灯片") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "立杆") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        CaptionNumberIN += Number(Numbers);
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
            });
        }

        var height = 228;
        //设施数量
        var CaptionNumberManager = 0
        if (CaptionNumberIN > 4) {
            CaptionNumberManager = 15;
        }
        //站址
        var byteLen = byteLength(item[i].StationAddress);
        var byteLenManager = 0;
        if (byteLen > 36) {
            byteLenManager = 15;
        }

        if (LineListleng <= 35) {
            height = 150 + CaptionNumberManager + byteLenManager;
        }
        if (LineListleng > 35 && LineListleng <= 68) {
            height = 168 + CaptionNumberManager + byteLenManager;
        }
        if (LineListleng > 68 && LineListleng <= 102) {
            height = 185 + CaptionNumberManager + byteLenManager;
        }
        if (LineListleng > 102 && LineListleng <= 125) {
            height = 201 + CaptionNumberManager + byteLenManager;
        }
        //height -= 20;
        var str_div = "<div id='RoundedCorner" + i + "' style='height:" + height + "px;width:280px;border-top:1px solid #7b9ebd;background-image:url(/Resource/images/imgs/03.png);border-left:1px solid #008d44;border-right:1px solid #008d44;margin-top:105px; margin-left:1px'>";
        str_div += "<div style=' width:100%; height:30px; z-index:100; background-image:url(/Resource/images/imgs/1.jpg);font-size:12px; line-height:30px; color:#fff; '><span style=' float:left; padding-left:10px'>公交站点：" + labl + "</span><img src='/Resource/Images/imgs/close.jpg' style=' float:right;text-decoration:none;margin-top:5px;cursor:pointer' onclick='colse(RoundedCorner" + i + ")'/></div>";
    

        str_div += " <div >"
        str_div += "<table style='font-size: 12px;'><tr><td style='width:60px'>站点站址</td><td style='width:220px;'>" + item[i].StationAddress + "</td></tr>";
        str_div += "<tr><td>停靠线路</td><td style='width:220px;'>" + strlineList + "</td></tr>";
        str_div += "<tr><td>站点编号</td><td style='width:220px;'><span href='javascript:;' onclick='javascript:linkztsystem(\"" + item[i].StopId + "\")'>" + item[i].StopId + "</span></td></tr>";
        str_div += "<tr><td>资产编号</td><td style='width:220px;'>" + CaptionNumberInfo + "</td></tr>";
        str_div += "<tr><td>站点照片</td><td style='width:220px' ><a href='javascript:;' onclick='javascript:openimage(\"" + item[i].StopId + "\")'>点击查看照片</a></td></tr>";
        str_div +="</table>";
        str_div += "<div style='display:none;'>[" + labl + "]</div></div>";
        if (Facilitiestxt != "" && Facilitiestxt != null && Facilitiestxt != undefined) {
            str_div += "<div style='height:20px; font-size: 15px;'>" + Facilitiestxt + "</div>";
        }
        else {
            str_div += "<div style='height:24px; font-size: 15px;'></div>";
        }

        str_div += "<div style='width:100%; background-image:url(/Resource/images/imgs/0300_02.png); height:69px;overflow:hidden; background-position:-10px' ></div></div>";

        if (strLatLon != null && strLatLon != "") {
            var m_window = new MInfoWindow("11", str_div);
            marker.info = m_window;
            marker.setLabel(new MLabel(labl), true); //添加标签
            marker.label.setVisible(false)//隐藏标签

            maplet.addOverlay(marker);
            MEvent.addListener(marker, "iw_beforeopen", showCustomIw);

        }


        tbl_temp += "<tr><td>" + (i + 1) + ".<a href='javascript:LatLonToCenter(" + strLatLon + ",false)'>" + item[i].StationName + item[i].StationAddress + "</a></td></tr>"
        //marker.bEditable = true;
        // 添加对marker 的事件
        MEvent.addListener(marker, "click", function (omarker) {
            omarker.openInfoWindow();
        });
        //omarker.
        MEvent.addListener(marker, "mouseover", function (mk) {
            if (mk.label) mk.label.setVisible(true);
        })
        MEvent.addListener(marker, "mouseout", function (mk) {
            if (mk.label) mk.label.setVisible(false);
        })
        points.push(marker);
        maplet.addOverlay(marker);
        //将地图移动到中心位置
        if (i == parseInt(item.length / 2)) {
            maplet.centerAndZoom(point, 10);
        }
    }
    tbl_temp += "</table></div>";
    $("#resultcontent").html(tbl_temp);

    //显示 站名
    showstationname(-1); ;
  }
//地图上面的点查询
function linksearchpoint(jsondata) {
    var rqurl = '/WebService.asmx/getPointsByLinkCdt';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var Jsonstr = eval('(' + callbackdata + ')');
    var item = eval('(' + Jsonstr.d + ')');
    removeAllOverlays();
    points = new Array();
    /*---请求地图数据end ---*/
    /*---将得到的地图数据放入表格中---*/
    var TotalDiv = "<div style='height:30px;top;20px; margin-left: 8px;'><table ><tr><td>站点数：" + item.length + "</td></tr></table></div>"; ;

    var tbl_temp = TotalDiv + "<div style='font-size:12px;'><table  Id='TblMap'>"
    for (var i = 0; i < item.length; i++) {
        var strLatLon = item[i].StrLatLon;
        if (strLatLon == "") {
            continue;
        }
        var lat = strLatLon.split(',')[0]; //经度
        var lon = strLatLon.split(',')[1]; //纬度
        var labl = item[i].StationName; //标签点的名称
        var point = new MPoint(lat, lon); //坐标点
        var PathDirection = item[i].PathDirection
        var w = 16;
        var h = 16;
        var icourl = "/images/icons/1/icon_03.png";
        switch (PathDirection) {
            case "枢纽":
                icourl = "/images/icons/1/icon_01.png";
                w = 40;
                h = 32;
                break;
            case "枢纽站":
                icourl = "/images/icons/1/icon_01.png";
                w = 40;
                h = 32;
                break;
            case "终点":
                w = 24;
                h = 24;
                icourl = "/images/icons/icon_02.png";
                break;
            case "终点站":
                w = 24;
                h = 24;
                icourl = "/images/icons/icon_02.png";
                break;
            default:
                icourl = "/images/icons/1/icon_03.png";
                break;
        }
        marker = new MMarker(point, new MIcon(icourl, w, h));
        var lineList = item[i].LineList.split("、");
        var strlineList = "";
        for (var j = 0; j < lineList.length; j++) {
            if (strlineList != "") {
                strlineList += "、<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[j] + "\",\"" + item[i].StopId + "\",\"" + strLatLon + "\")'>" + lineList[j] + "</a>"

            } else {
                strlineList += "<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[j] + "\",\"" + item[i].StopId + "\",\"" + strLatLon + "\")'>" + lineList[j] + "</a>"
            }
        }
        var CaptionNumberInfo = getCaptionNumberInfo(item[i]);
        var str_div = "<div class='RoundedCorner'>"
        + "<table id='tablereg'><tr><td style='width:60px'>站点站址</td><td style='width:240px;'>" + item[i].StationAddress + "</td></tr><tr><td>停靠线路</td><td>" + strlineList + "</td></tr><tr><td>站点编号</td><td><span href='javascript:;' onclick='javascript:linkztsystem(\"" + item[i].StopId + "\")'>" + item[i].StopId + "</span></td></tr>"
        + "<tr><td>资产编号</td><td>" + CaptionNumberInfo + "</td></tr>"
       + "<tr><td>站点照片</td><td><a href='javascript:;' onclick='javascript:openimage(\"" + item[i].StopId + "\")'>点击查看照片</a></td></tr>"
        + "</table>"
        + "<div style='display:none;'>[" + labl + "]<div></div>";
        marker.info = new MInfoWindow("当前站点:" + labl, str_div, "20px", "20px");
        marker.setLabel(new MLabel(labl), true); //添加标签
        marker.label.setVisible(false)//隐藏标签
        ////
        tbl_temp += "<tr><td>" + (i + 1) + ".<a href='javascript:LatLonToCenter(" + strLatLon + ",false)'>" + item[i].StationName + item[i].StationAddress + "</a></td></tr>"
        //marker.bEditable = true;
        // 添加对marker 的事件
        MEvent.addListener(marker, "click", function (omarker) {
            omarker.openInfoWindow();
        });
        //omarker.
        MEvent.addListener(marker, "mouseover", function (mk) {
            if (mk.label) mk.label.setVisible(true);
        })
        MEvent.addListener(marker, "mouseout", function (mk) {
            if (mk.label) mk.label.setVisible(false);
        })
        points.push(marker);
        maplet.addOverlay(marker);
        //将地图移动到中心位置
        if (i == parseInt(item.length / 2)) {
            maplet.centerAndZoom(point, 10);
        }
    }
    tbl_temp += "</table></div>";
    $("#resultcontent").html(tbl_temp);

    //显示 站名
    showstationname(-1); ;
}

function linkztsystem(StopId) {
    return false;
    //得到系统域自动判断跳转地址
    var BASEDOMAIN = window.location.hostname;
    var url = "";
    url += "http://" + BASEDOMAIN + "/FunctionModule/InformationCenter/frmStationStop.aspx?StopId=" + StopId;
    window.open(url);
}
function getCaptionNumberInfo(oStop) {
    var stationinfo = "";
    var polesinfo = "";
    if (oStop.FacilitiesList != undefined) {
        var Facs = oStop.FacilitiesList;
        $(Facs).each(function (k) {
            if (Facs[k].FacilitiesType == "候车亭") {
                if (stationinfo != "") {
                    stationinfo += "、" + Facs[k].CapitalNumber;
                } else {
                    stationinfo += Facs[k].CapitalNumber;
                }

            }
            else {
                if (polesinfo != "") {
                    polesinfo += "、" + Facs[k].CapitalNumber;
                } else {
                    polesinfo += Facs[k].CapitalNumber;
                }
            }
        });
    }
    var CaptionNumberInfo = "";
    if (stationinfo != "") {
      
        CaptionNumberInfo += "<a onclick=\"showImage('" + stationinfo + "')\">" + stationinfo + "（亭）</a>"
    }
    if (polesinfo != "") {
        CaptionNumberInfo += polesinfo + "（牌）"
    }
    return CaptionNumberInfo;
}

function showImage(id) {
    $.getJSON('/MapSearch/ImageList?id=' + id, function (json) {
       layer.photos({
            photos: json //格式见API文档手册页
        });
    });
}

//获得创建的线路头
function getCreateHeader(RoadLine, RunTime, RunCompany, ToDirection, TicketStyle, rowscount) {
    var stime = RunTime.split("-")[0];
    var etime = RunTime.split("-")[1];
    var lineheight = "line-height:77px;";
    if (RoadLine.length > 3) {
        lineheight = "";
    }
    var strdiv = "<div style=\'background-color: #A6CE39; height: 129px; /*width: 262px;*/\'>"
       + "<div id=\'Div3\' style=\'background-color: Transparent; color: White; font-size: 30px;"
       + "position: relative; left: 0px; top: 0px; width: 144px; height: 77px; font-weight: bold; text-align:center;" + lineheight + "\'>"
       + "<span class=\'style1\'>" + RoadLine + "</span></div>"
       + "<div style=\'width: 94px; position: relative; top: -65px; left: 163px; font-size: 13px;  height: 59px;\'>"
       + "<span class=\'style1\'>首班车 " + stime + "</span><br /><span class=\'style1\'>末班车 " + etime + "</span><br />"
       + "<span class=\'style1\'>" + RunCompany + "</span></div>"
       + "<div style=\'width: 244px; position: relative; top: -53px; left: 4px; color: White;"
       + "font-size: 18px; font-weight: bold; height: 23px;\'>"
       + "<span class=\'style1\' style='font-weight: bold;'>开往 " + ToDirection + "</span></div>"
       + "<div style=\'width: 100%; position: relative; top: -49px; left: 4px; font-size: 10px;right: -4px; height: 11px;\'>"
       + "<span class=\'style1\' style='font-weight: bold;'>" + TicketStyle + " 本线无人售票请自备零钱不设零找</span></div> "
       + " <div id=\'divline\' style=\'width: 11px; position: relative; top: -40px; left: 34px;height:" + (17.8 * rowscount) + "px;"
       + " background: url(\"/images/line.png\"); border: none;\'></div>"
       + "<div id=\'divarror\' style=\'width: 17px; height: 56px; position: relative; top: -40px; left: 28px;"
       + " background: url(\"/images/arror.png\"); border: none;\'></div></div>";

    return strdiv;
}
function ToPoint(strTable, rowindex) {
    $("#" + strTable.id + " tr").each(function (i) {
        var tcell = $(this).get(0).cells.item(0);
        if (i == rowindex) {
            tcell.style.backgroundImage = 'url(/Resource/images/point.png)';
            tcell.style.width = "12px";
            tcell.style.backgroundRepeat = 'no-repeat';
            var tcell2 = $(this).get(0).cells.item(2);
            if (tcell2.getElementsByTagName("A") != null) {
                tcell2.getElementsByTagName("A")[0].style.color = "Red";
            }
        }
        else {

            tcell.style.backgroundImage = "";
            if (i < rowindex) {
                var tcell = $(this).get(0).cells.item(2);
                if (tcell.getElementsByTagName("A") != null) {
                    tcell.getElementsByTagName("A")[0].style.color = "Gray";
                }

            }
            else if (i > rowindex) {
                var tcell = $(this).get(0).cells.item(2);
                tcell.getElementsByTagName("A")[0].style.color = "Black";
                if (i == $("#" + strTable.id + " tr").size() - 1) {
                    tcell.getElementsByTagName("A")[0].style.color = "Blue";
                }
            }

        }
    });
}
function LoadRelationsRoadLine(data, textStatus) {
    var item = $.parseJSON($.parseJSON(data).d);
    //用于存放当前点的集合的数组
    var strlinelatlon = "";
    if (item.LineLatLon != undefined) {
        var points_temp = new Array();
        var LineLatLon = item.LineLatLon.split(';')
        for (var i = 0; i < LineLatLon.length; i++) {

            if (LineLatLon[i] != null && LineLatLon[i] != "" && LineLatLon[i] != undefined) {
                if (i > 0) {
                    strlinelatlon += ";" + LineLatLon[i];
                }
                else {
                    strlinelatlon += LineLatLon[i];
                }
                var lat = LineLatLon[i].split(',')[0];
                var lon = LineLatLon[i].split(',')[1];
                var point = new MPoint(lat, lon);
                points_temp.push(point);
            }
        }
        //创建笔刷将所有的点连接起来
        var brush = new MBrush();
        brush.arrow = false;
        brush.stroke = 4;
        brush.fill = false;
        brush.color = getRandomColor();
        brush.bgcolor = 'red';
        var polyline = new MPolyline(points_temp, brush);
        maplet.addOverlay(polyline);
    }
}
/*折线部分全局参数*/
//setBrush(new_brush)
var lines = null;
var index = 0;
function SearchRoadsByCompany() {
    lines = null;
    index = 0;
    var RunCompany = $("#slRunCompany").val();
    if (RunCompany == "-营运公司-") {
        window.alert("请选择一个营运公司！");
        return false;
    }
    removeAllOverlays();
    var jsondata = '{RunCompany:"' + RunCompany + '"}';
    var rqurl = '/WebService.asmx/getRoadsByRunCompany';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var Jsonstr = eval('(' + callbackdata + ')');
    callbackdata = null;
    var items = Jsonstr.d;
    Jsonstr = null;
    if (items == null) {
        alert('查询无数据！');
        return false;
    }

    var RoadInfo = eval('(' + items + ')');
    items = null;
    var TotalDiv = "<div style='height:30px;top;20px; margin-left: 8px;'><table ><tr><td>营运公司:" + RunCompany + " 包含线路数：" + RoadInfo.RoadCount + "</td></tr></table></div>"; ;
    Roads = RoadInfo.RoadLines;
    var tbl_temp = TotalDiv + "<div style='font-size:12px;'><table  Id='TblMap'>"
    lines = new Array();
    for (var i = 0; i < Roads.length; i++) {
        var Road = Roads[i];
        var RoadLine = Road["RoadLine"];
        var ToDirection = Road["ToDirection"];
        var RunCompany = Road["RunCompany"];
        var LineLatlon = Road["LineLatlon"];
        var Center = Road["Center"];
        if (i == 0) {
            tbl_temp += "<tr><th>编号</th><th>线路名称</th></tr>"
        }
        /*onmousemove=\'javascript:HighlightLine(" + i + ")\' onmouseout=\'javascript:hiddenLine(" + i + ")\'*/
        tbl_temp += "<tr><td>" + (i + 1) + "</td><td><a href=\'javascript:;\'  onclick=\'javascript:ShowLevel(" + i + ")\'>" + RoadLine + "-" + ToDirection + "</a></td></tr>";
        var points_temp = new Array();
        if (LineLatlon != null) {
            var LineLatlons = LineLatlon.split(';');
            for (var j = 0; j < LineLatlons.length; j++) {
                if (LineLatlons[j] != null && LineLatlons[j] != "" && LineLatlons[j] != undefined) {
                    var lat = LineLatlons[j].split(',')[0];
                    var lon = LineLatlons[j].split(',')[1];
                    var point = new MPoint(lat, lon);
                    points_temp.push(point);
                }
            }
            //创建笔刷将所有的点连接起来
            var brush = new MBrush();
            brush.arrow = false;
            brush.stroke = 2;
            brush.fill = false;
            brush.color = 'blue';
            brush.bgcolor = 'red';
            var polyline = new MPolyline(points_temp, brush);
            lines.push(polyline);
        }
    }
    tbl_temp += "</table></div>";
    $("#resultcontent").html(tbl_temp);
    tbl_temp = null;
    timerId = window.setInterval(showline, 100);
}
function showline() {
    if (index < lines.length - 1) {
        var polyline = lines[index];
        maplet.addOverlay(polyline);
        index++;

    } else {
        stopshow();
    }
}
function stopshow() {
    if (timerId) window.clearInterval(timerId);
    counter = 0;
}
function HighlightLine(i) {
    if (i < lines.length) {
        var line = lines[i];
        maplet.removeOverlay(line, false);
        var brush = new MBrush();
        brush.arrow = false;
        brush.stroke = 5;
        brush.fill = false;
        brush.color = 'red';
        brush.bgcolor = 'red';
        line.setBrush(brush);
        maplet.addOverlay(line);
        var point = line.getCenterPt();
        if (point != null) {
            maplet.centerAndZoom(point, /*maplet.getZoomLevel()*/14);
        }
    }
}
function hiddenLine(i) {
    if (i < lines.length) {
        var line = lines[i];
        maplet.removeOverlay(line, false);
        var brush = new MBrush();
        brush.arrow = false;
        brush.stroke = 2;
        brush.fill = false;
        brush.color = 'blue';
        brush.bgcolor = 'red';
        line.setBrush(brush);
        maplet.addOverlay(line);
    }
}
var pointLevelList = [];
var lastRoadline=-1;
function ShowLevel(i) {

    if (pointLevelList.length > 0) {
        for (var ii = 0; ii < pointLevelList.length; ii++) {
            maplet.removeOverlay(pointLevelList[ii], false);
        }
    }
    if (lastRoadline != -1) {
        hiddenLine(lastRoadline);
    }
    lastRoadline = i;
    HighlightLine(i);
    if (i < Roads.length) {
        var line = Roads[i];
        var levelList = line.levellist;
        for (var ii = 0; ii < levelList.length; ii++) {
            var level = levelList[ii];
            var lonlat=level.LonLat.split(',');
            var lon = lonlat[0];
            var lat = lonlat[1];
            if (lon != "" && lat != "") {
                var label = level.stationname;
                var marker = new MMarker(new MPoint(lon, lat), new MIcon("/images/icon_03.png", 16, 16));
                marker.setLabel(new MLabel(label), true);
                marker.label.setVisible(false)//隐藏标签
                pointLevelList.push(marker);

                var str_div = "<div style='font-size:12px;'><table><tr><td style='width:60px;'>当前线路</td><td>" + level.roadline + "</td></tr><tr><td style='width:60px;'>开发方向</td><td>" + level.ToDirection + "</td></tr><tr><td style='width:60px;'>站点站址</td><td>" + level.addr + "</td></tr><tr><td>停靠线路</td><td>" + level.linelist + "</td></tr><tr><td>站点编号</td><td>" + level.stopid + "</a></td></tr></table></div>";
                marker.info = new MInfoWindow("当前站点:" + label, str_div);

                MEvent.addListener(marker, "click", function (omarker) {

                    omarker.openInfoWindow();
                });
                MEvent.addListener(marker, "mouseover", function (mk) {
                    if (mk.label) mk.label.setVisible(true);
                })
                MEvent.addListener(marker, "mouseout", function (mk) {
                    if (mk.label) mk.label.setVisible(false);
                })
               
                maplet.addOverlay(marker);




            }
        
        }
    }
}
var getRandomColor = function () {
    return '#' + (function (h) {
        return new Array(7 - h.length).join("0") + h
    })((Math.random() * 0x1000000 << 0).toString(16))
}
function getRoadLineColor(num) {
    var colorname;
    switch (num) {
        case 0:
            colorname = "Red";
            break;
        case 1:
            colorname = "Blue";
            break;
        case 2:
            colorname = "Green";
            break;
        case 3:
            colorname = "Ivory";
            break;
        case 4:
            colorname = "Teal";
            break;
        case 5:
            colorname = "Pink";
            break;
        case 6:
            colorname = "Orchid";
            break;
        case 7:
            colorname = "DarkBlue";
            break;
        default:
            colorname = "Blue";
            break;
    }
    return colorname;
}
function SearchStopId() {
    clearMap();
    points = new Array();
    $("#resultcontent").html("");
    var stopId = $('#sole-input').val();
    var rqurl = '/WebService.asmx/getStopIdPonintInfo';
   
    var jsondata = '{ stopid:"'+ stopId+'" }';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var Jsonstr = $.parseJSON(callbackdata);
    var item = $.parseJSON(Jsonstr.d);
    if (item == null) {
        return false;
    }
    points = new Array();
    PointsTr = "";
    /*---请求地图数据end ---*/
    /*---将得到的地图数据放入表格中---*/
    var RoadlineCount = "";
    var StationCount = "";
    var PolesCount = "";
    var LcdCount = "";
    var LampCount = "";
    var StopIdlist = "";

    for (var i = 0; i < item.length; i++) {
        var marker = null;
        var labl = item[i].StationName; //标签点的名称
        if (StopIdlist.length > 0) {
            StopIdlist += "," + item[i].StopId;
        } else {
            StopIdlist += item[i].StopId;
        }
        var strLatLon = item[i].Strlatlon;
        if (strLatLon != null && strLatLon != "") {
            var lat = strLatLon.split(',')[0]; //经度
            var lon = strLatLon.split(',')[1]; //纬度
            var point = new MPoint(lat, lon); //坐标点
            var PathDirection = item[i].PathDirection
            var w = 16;
            var h = 16;
            var icourl = "/images/icons/1/icon_03.png";
            switch (PathDirection) {
                case "枢纽":
                    icourl = "/images/icons/1/icon_01.png";
                    w = 40;
                    h = 32;
                    break;
                case "枢纽站":
                    w = 40;
                    h = 32;
                    icourl = "/images/icons/1/icon_01.png";
                    break;
                case "终点":
                    w = 24;
                    h = 24;
                    icourl = "/images/icons/1/icon_02.png";
                    break;
                case "终点站":
                    w = 24;
                    h = 24;
                    icourl = "/images/icons/1/icon_02.png";
                    break;
                default:
                    icourl = "/images/icons/1/icon_03.png";
                    break;
            }
            marker = new MMarker(point, new MIcon(icourl, w, h));
        }
        //        var new_shado = new MIconShadow("/images/icons/shadow.png", 16, 16, -14)
        //        marker.setShadow(new_shado, true)
        var lineList = item[i].LineList.split("、");
        var strlineList = "";

        for (var j = 0; j < lineList.length; j++) {
            if (strlineList != "") {
                strlineList += "、<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[j] + "\",\"" + item[i].StopId + "\",\"" + strLatLon + "\")'>" + lineList[j] + "</a>"

            } else {
                strlineList += "<a href='javascript:;' onclick='javascript:searchroadline(\"" + lineList[j] + "\",\"" + item[i].StopId + "\",\"" + strLatLon + "\")'>" + lineList[j] + "</a>"
            }
        }

        var LineListleng = item[i].LineListleng;
        RoadlineCount = item[i].RoadlineCount;
        StationCount = item[i].StationCount;
        PolesCount = item[i].PolesCount;
        LcdCount = item[i].LcdCount;
        LampCount = item[i].LampCount;
        var CaptionNumberIN = 0;
        var CaptionNumberInfo = getCaptionNumberInfo(item[i]);
        var Facilitiestxt = "";
        if (item[i].FacilitiesSUM != undefined) {
            var Facs = item[i].FacilitiesSUM;
            $(Facs).each(function (k) {
                var FacilitiesType = Facs[k].FacilitiesType;
                var Numbers = Facs[k].Number;
                if (FacilitiesType == "无电候车亭") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        CaptionNumberIN += Number(Numbers);
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_05.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "有电候车亭") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        CaptionNumberIN += Number(Numbers);
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_06.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "视频") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_04.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "灯片") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_08.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
                if (FacilitiesType == "立杆") {
                    if (Numbers != "0") {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>" + Numbers + "&nbsp;&nbsp;";
                        CaptionNumberIN += Number(Numbers);
                    }
                    else {
                        Facilitiestxt += "<img src='/Resource/Images/icons/1/icon_07.png' style='vertical-align:middle;'/>&nbsp;&nbsp;";
                    }
                }
            });
        }
        var height = 228;
        //设施数量
        var CaptionNumberManager = 0
        if (CaptionNumberIN > 4) {
            CaptionNumberManager = 15;
        }
        //站址
        var byteLen = byteLength(item[i].StationAddress);
        var byteLenManager = 0;
        if (byteLen > 36) {
            byteLenManager = 15;
        }

        if (LineListleng <= 35) {
            height = 145 + CaptionNumberManager + byteLenManager;
        }
        if (LineListleng > 35 && LineListleng <= 68) {
            height = 163 + CaptionNumberManager + byteLenManager;
        }
        if (LineListleng > 68 && LineListleng <= 102) {
            height = 180 + CaptionNumberManager + byteLenManager;
        }
        if (LineListleng > 102 && LineListleng <= 125) {
            height = 196 + CaptionNumberManager + byteLenManager;
        }
        ////height -= 20;

        var str_div = "<div id='RoundedCorner" + i + "' style='height:" + height + "px;width:280px;border-top:1px solid #7b9ebd;background-image:url(/Resource/images/imgs/03.png);border-left:1px solid #008d44;border-right:1px solid #008d44;margin-top:105px; margin-left:1px'>";
        str_div += "<div style=' width:100%; height:30px; z-index:100; background-image:url(/Resource/images/imgs/1.jpg);font-size:12px; line-height:30px; color:#fff; '><span style=' float:left; padding-left:10px'>公交站点：" + labl + "</span><img src='/Resource/Images/imgs/close.jpg' style=' float:right;text-decoration:none;margin-top:5px;cursor:pointer' onclick='colse(RoundedCorner" + i + ")'/></div>";

        str_div += " <div >"
        str_div += "<table style='font-size: 12px;'><tr><td style='width:60px'>站点站址</td><td style='width:220px;'>" + item[i].StationAddress + "</td></tr>";
        str_div += "<tr><td>停靠线路</td><td >" + strlineList + "</td></tr>";
        str_div += "<tr><td>站点编号</td><td ><span href='javascript:;' onclick='javascript:linkztsystem(\"" + item[i].StopId + "\")'>" + item[i].StopId + "</span></td></tr>";
        str_div += "<tr><td>资产编号</td><td>" + CaptionNumberInfo + "</td></tr>";
        str_div += "<tr><td>站点照片</td><td ><a href='javascript:;' onclick='javascript:openimage(\"" + item[i].StopId + "\")'>点击查看照片</a></td></tr>";
        str_div += "</table>";
        str_div += "<div style='display:none;'>[" + labl + "]</div></div>";
        if (Facilitiestxt != "" && Facilitiestxt != null && Facilitiestxt != undefined) {
            str_div += "<div style='height:20px; font-size: 15px;'>" + Facilitiestxt + "</div>";
        }
        else {
            str_div += "<div style='height:24px; font-size: 15px;'></div>";
        }

        str_div += "<div style='width:100%; background-image:url(/Resource/images/imgs/0300_02.png); height:69px;overflow:hidden; background-position:-10px' ></div></div>";



        if (strLatLon != null && strLatLon != "") {
            var m_window = new MInfoWindow("11", str_div);
            marker.info = m_window;
            marker.setLabel(new MLabel(labl), true); //添加标签
            marker.label.setVisible(false)//隐藏标签

            maplet.addOverlay(marker);
            MEvent.addListener(marker, "iw_beforeopen", showCustomIw);
        }
        var index = 0;
        if (!$("#cbxIsAdd").attr("checked")) {
            index = (i + 1);
        }
        else {
            index = PointCount + (i + 1)
        }
        PointsTr += "<tr><td>" + index + ".<a href='javascript:LatLonToCenter(" + strLatLon + ",false)'>" + item[i].StationName + item[i].StationAddress + "</a></td></tr>"
        //marker.bEditable = true;
        // 添加对marker 的事件
        if (strLatLon != null && strLatLon != "") {
            MEvent.addListener(marker, "click", function (omarker) {
                omarker.openInfoWindow();
            });
            //omarker.
            MEvent.addListener(marker, "mouseover", function (mk) {
                if (mk.label) mk.label.setVisible(true);
            })
            MEvent.addListener(marker, "mouseout", function (mk) {
                if (mk.label) mk.label.setVisible(false);
            })
            points.push(marker);
            maplet.addOverlay(marker);
        }
        if (strLatLon != null && strLatLon != "") {
            //将地图移动到中心位置
            //            maplet.centerAndZoom(point, 20);
            if (i == parseInt(item.length / 2)) {
                maplet.centerAndZoom(point, 20);
            }
        }
    }
    if (!$("#cbxIsAdd").attr("checked")) {
        PointCount = item.length;
    }
    else {
        PointCount += item.length;
    }
    var tbl_temp = "<div style='height:30px;top;20px; margin-left: 8px;'><table ><tr><td>站点数:" + PointCount + ";";
    if (RoadlineCount != null && RoadlineCount != undefined && RoadlineCount != "" && RoadlineCount != "0") {
        //tbl_temp += "线路数:<a href='javascript:;' onclick='javascript:openimageRoadline(\"" + StopIdlist + "\")'>" + RoadlineCount + "</a>;";
        tbl_temp += "线路数:" + RoadlineCount + ";";
    }
    if (StationCount != null && StationCount != undefined && StationCount != "" && StationCount != "0") {
        tbl_temp += "候车亭数:" + StationCount + ";";
    }
    if (PolesCount != null && PolesCount != undefined && PolesCount != "" && PolesCount != "0") {
        tbl_temp += "立杆数:" + PolesCount + ";";
    }
    if (LcdCount != null && LcdCount != undefined && LcdCount != "" && LcdCount != "0") {
        tbl_temp += "视频:<a href='javascript:;' onclick='javascript:openimageLcd(\"" + StopIdlist + "\")'>" + LcdCount + "</a>;";
    }
    if (LampCount != null && LampCount != undefined && LampCount != "" && LampCount != "0") {
        tbl_temp += "灯片:<a href='javascript:;' onclick='javascript:openimageLamp(\"" + StopIdlist + "\")'>" + LampCount + "</a>;";
    }
    tbl_temp += "</td></tr></table></div><div style='font-size:12px;'><table  Id='TblMap'>";
    tbl_temp += PointsTr;
    tbl_temp += "</table></div>";
    $("#resultcontent").html(tbl_temp);
    //显示 站名
    showstationname(-1); ;
}
function SearchByKeyword() {

    clearMap();
    points = new Array();
    $("#resultcontent").html("");
    var stopId = $('#sole-input').val();
    var rqurl = '/WebService.asmx/getResultByKeyword';

    var jsondata = '{ stopid:"' + stopId + '" }';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var Jsonstr = $.parseJSON(callbackdata);
    var item = $.parseJSON(Jsonstr.d);
    if (item == null) {
        return false;
    }
    points = new Array();

}

