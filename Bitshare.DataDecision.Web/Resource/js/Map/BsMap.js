/// <reference path="publicjs/jquery-1.4.1-vsdoc.js" />
//Jquery AJAX 请求webserive 数据
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



var getRandomColor = function () {
    return '#' + (function (h) {
        return new Array(7 - h.length).join("0") + h
    })((Math.random() * 0x1000000 << 0).toString(16))
}
var maplet = null; //地图对象
var marker = null; //标注对象
var menu = null;
var indexcity = "上海市";
var apiType = 1; //是否为明文经纬度
//初始化地图
function initMap2() {
    try {
        maplet = new Maplet("mapbar");
    }
    catch (ex) {
        window.alert("地图加载失败，请刷新重试！");
        return;
    }
    //maplet.customInfoWindow = true;
    maplet.centerAndZoom(new MPoint(indexcity), 8); //设置默认城市和缩放级别
    maplet.addControl(new MStandardControl());
    maplet.showLogo(false); //隐藏Logo 等等一些信息
    maplet.showNavLogo(false);
    // maplet.customInfoWindow = true;
    maplet.clickToCenter = false; //点击了之后是否将点击的位置设置为中心点
    maplet.setAutoZoom(); //设置根据地图自动适应
    maplet.setOverviewLocation({ type: eval("Maplet.RIGHT_BOTTOM") })//设置鹰眼的位置
    var width = $(window).width() - 356; //document.documentElement.clientWidth - 10.5;
    var topHeight = $('.container-fluid div:first').height();
    var height = $(window).height() - topHeight - 11;
    maplet.resize(width, height); //地图初始化时候的大小
    CreateMenu();
    $("#hideResult").click();
}
function initMap() {
    // alert(1);
    try {
        maplet = new Maplet("mapbar");
    }
    catch (ex) {
        window.alert("地图加载失败，请刷新重试！");
        return;
    }
    maplet.centerAndZoom(new MPoint(indexcity), 8); //设置默认城市和缩放级别
    maplet.addControl(new MStandardControl());

    maplet.showLogo(false); //隐藏Logo 等等一些信息
    maplet.showNavLogo(false);
    maplet.clickToCenter = false; //点击了之后是否将点击的位置设置为中心点
    maplet.setAutoZoom(); //设置更具地图自动适应
    //  maplet.customInfoWindow = true;
    maplet.setOverviewLocation({ type: eval("Maplet.RIGHT_BOTTOM") })//设置鹰眼的位置
    var width = $(window).width(); //document.documentElement.clientWidth - 10.5;
    var topHeight = $('.container-fluid div:first').height();
    var height = $(window).height() - topHeight - 11;
   
    maplet.resize(width, height); //地图初始化时候的大小

    CreateMenu();

}
function initMap1() {
    try {
        maplet = new Maplet("mapbar");
    }
    catch (ex) {
        window.alert("地图加载失败，请刷新重试！");
        return;
    }
    //maplet.customInfoWindow = true;
    maplet.centerAndZoom(new MPoint(indexcity), 8); //设置默认城市和缩放级别
    maplet.addControl(new MStandardControl());
    maplet.showLogo(false); //隐藏Logo 等等一些信息
    maplet.showNavLogo(false);
    maplet.clickToCenter = false; //点击了之后是否将点击的位置设置为中心点
    maplet.setAutoZoom(); //设置根据地图自动适应
    maplet.setOverviewLocation({ type: eval("Maplet.RIGHT_BOTTOM") }); //设置鹰眼的位置
    maplet.showOverview(true, false)
    var width = $(window).width(); //document.documentElement.clientWidth - 10.5;
    var height = $(window).height();
    maplet.resize(width, height); //地图初始化时候的大小
    CreateMenu();

}
function ToshowChice() {
    $("#showChice").show();
    $("#showMap").hide();
}
function fullscreen(obj) {
    if (obj.innerHTML == "全屏") {
        obj.innerHTML = "还原"
        $("#left").css("display", "none");
        $("#mapbar").removeClass("mapObj2").removeClass("mapObj").addClass("mapObj3");
        $("#mapTitle").removeClass("title2").removeClass("title").addClass("title3");
        $("#hideResult").removeClass("hideResult").addClass("hideResult2");
        $("#logo").removeClass("logo").addClass("logo1");
        $("#logoBk").removeClass("logoBk").addClass("logoBk2");
        var width = $(window).width() - 3;
        var height = $(window).height() - 33;
        maplet.resize(width, height); //地图初始化时候的大小
    }
    else {
        obj.innerHTML = "全屏"
        $("#left").css("display", "block");
        $("#mapbar").removeClass("mapObj3").addClass("mapObj");
        $("#mapTitle").removeClass("title3").addClass("title");
        $("#hideResult").removeClass("hideResult2").addClass("hideResult");
        $("#logo").removeClass("logo1").addClass("logo");
        $("#logoBk").removeClass("logoBk2").addClass("logoBk")
        var width = $(window).width() - 356; //document.documentElement.clientWidth - 10.5;
        var height = $(window).height() - 130;
        maplet.resize(width, height);
    }

}
$(window).resize(function () {
    if ($("#mapbar").css("top") == "30px") {

        var width = $(window).width() - 3;
        var topHeight = $('.container-fluid div:first').height();
        var height = $(window).height() - topHeight - 11;
        maplet.resize(width, height); //地图初始化时候的大小
    }
    else {
        var width = $(window).width() - 11;
        if ($("#left").css("display") == "block") {
            width = $(window).width() - 356;
        }
        else {

        } //document.documentElement.clientWidth - 10.5;
        var topHeight = $('.container-fluid div:first').height();
        var height = $(window).height() - topHeight - 11;
        if (maplet != null) {
            maplet.resize(width, height); //地图初始化时候的大小
        }
    }


});
function CreateMenu() {
    menu = new MContextMenu(); //菜单
    var menuitem_del = new MContextMenuItem("删除位置", DelPoint);
    var menuitem_edit = new MContextMenuItem("编辑位置", EditPoint);
    //    var menuitem_move = new MContextMenuItem("移动点", MovePoint);
    menu.addItem(menuitem_del);
    menu.addItem(menuitem_edit);
    //    menu.addItem(menuitem_move);
}
function EditPoint(contextMenuItem, contextMenu, overlay) {
    avBubble.width = 280;
    avBubble.height = 240;
    $('#rdMoveMap').click();
    $("#txtStrLatlon").val(overlay.pt.getPid());
    var strdiv = $("#divcontent").html();
    var m_window = new MInfoWindow("编辑位置信息", strdiv);
    overlay.info = m_window;
    overlay.openInfoWindow();
   
}
function DelPoint(contextMenuItem, contextMenu, overlay) {
    if (confirm("确定要删除点吗?")) {
        setCurrentMouseTool("pan");
        overlay.remove(true);
    }
}
function MovePoint(contextMenuItem, contextMenu, overlay) {
    if (confirm("确定要移动位置吗?")) {
        setCurrentMouseTool("pan");
        overlay.bEditable = true;
    }
}
function openimage(stopid) {

    window.open("/ReportNew/showImages.aspx?StopId=" + stopid);
}
function selectitem(item) {
    var itemtext = item.options[item.selectedIndex].text;
    var c_id = item.id;
    if (itemtext != "-请选择-") {
        if (c_id == "sltArea") {
            $("#sltDistrict").empty();
            var options = "<option value='-请选择-'>-请选择-</option>";
            var str_where = "Area='" + itemtext + "'";
            var jsondata = '{table_name:"tblStop3",str_field:"District",str_where:"' + str_where + '"}';
            var rqurl = 'WebService.asmx/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            var item = Jsonstr.d;
            if (item == null) {
                return false;
            }

            for (var i = 0; i < item.length; i++) {
                options += "<option value=" + item[i] + ">" + item[i] + "</option>";
            }
            $(options).appendTo($("#sltDistrict")); //
        }
        else if (c_id == "sltDistrict") {
            $("#sltRoadName").empty();
            var options = "<option value='-请选择-'>-请选择-</option>";
            var str_where = "District='" + itemtext + "'";
            var jsondata = '{table_name:"tblStop3",str_field:"RoadName",str_where:"' + str_where + '"}';
            var rqurl = 'WebService.asmx/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            var item = Jsonstr.d;
            if (item == null) {
                return false;
            }
            for (var i = 0; i < item.length; i++) {
                options += "<option value=" + item[i] + ">" + item[i] + "</option>";
            }
            $(options).appendTo($("#sltRoadName")); //
        }
        else if (c_id == "sltRoadName") {
            $("#sltStationName").empty();
            var options = "<option value='-请选择-'>-请选择-</option>";
            var str_where = "RoadName='" + itemtext + "'";
            var jsondata = '{table_name:"tblStop3",str_field:"StationName",str_where:"' + str_where + '"}';
            var rqurl = 'WebService.asmx/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            var item = Jsonstr.d;
            if (item == null) {
                return false;
            }
            for (var i = 0; i < item.length; i++) {
                options += "<option value=" + item[i] + ">" + item[i] + "</option>";
            }
            $(options).appendTo($("#sltStationName")); //
        }
        else if (c_id == "sltStationName") {
            $("#sltPathDirection").empty();
            var options = "<option value='-请选择-'>-请选择-</option>";
            var str_where = "StationName='" + itemtext + "'";
            var jsondata = '{table_name:"tblStop3",str_field:"PathDirection",str_where:"' + str_where + '"}';
            var rqurl = 'WebService.asmx/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            var item = Jsonstr.d;
            if (item == null) {
                return false;
            }
            for (var i = 0; i < item.length; i++) {
                options += "<option value=" + item[i] + ">" + item[i] + "</option>";
            }
            $(options).appendTo($("#sltPathDirection")); //
        }
        else if (c_id == "sltPathDirection") {
            $("#sltNumber").empty();
            var options = "<option value='-请选择-'>-请选择-</option>";

            var str_where = "StationName='" + $("#sltStationName").attr("value") + "' AND PathDirection='" + itemtext + "'";
            var jsondata = '{table_name:"tblStop3",str_field:"StopId",str_where:"' + str_where + '"}';
            var rqurl = 'WebService.asmx/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            var item = Jsonstr.d;
            if (item == null) {
                return false;
            }
            for (var i = 0; i < item.length; i++) {
                options += "<option value=" + item[i] + ">" + item[i] + "</option>";
            }
            $(options).appendTo($("#sltNumber"))//
        }
    }
}
function SavePoint() {
    var strlatlon = $("#txtlatlon").attr("value");
    var stopid = $("#sltNumber").attr("value");
    if (stopid == "-请选择-") {
        window.alert("请选择站点编号！");
        return false;
    }
    var str_where = "StopId=" + stopid;
    var field_value = "StrLatlon='" + strlatlon + "'";
    var jsondata = '{table_name:"tblStop3",field_value:"' + field_value + '",str_where:"' + str_where + '"}';
    var rqurl = 'WebService.asmx/SaveToTblStop3';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var Jsonstr = eval('(' + callbackdata + ')');
    var item = Jsonstr.d;
    if (item == null) {
        return false;
    }
    if (item) {
        alert("更新成功！");
    }
    else {
        alert("更新失败！");
    }
}
function SearchPoint(contextMenuItem, contextMenu, overlay) {
    setCurrentMouseTool("pan");
    // maplet.setMode("");
    var str_div = "";
    var str_div = "<div>"
        + "这里放广告相关的信息"
        + "</div>";
    var tabs = new Array();
    tabs.push(new MInfoWindowTab("当前位置信息", str_div));
    var m_window = new MInfoWindow("位置信息", "");
    m_window.setZMBtnVisible(false);
    m_window.setTabs(tabs);
    overlay.info = m_window;
    overlay.openInfoWindow();
}
//变为移动模式
function setCurrentMouseTool(mode_name) {
    maplet.setMode(mode_name);
}
//城市位置切换
function AddressToCenter(address) {
    maplet.centerAndZoom(new MPoint(address), 14);
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
function removeAllOverlays() {
    maplet.clearOverlays(true);
    maplet.refresh();
}
function SetCursorStyle() {
    //maplet.setCursorStyle("default","image/larrow.cur");
    //maplet.setCursorStyle("pointer","image/lnodrop.cur");
    //maplet.setCursorStyle("move","image/lmove.cur");
    //maplet.setCursorStyle("crosshair","image/lwait.cur");
}
function getQueryString(name) {     // 如果链接没有参数，或者链接中不存在我们要获取的参数，直接返回空    
    if (location.href.indexOf("?") == -1 || location.href.indexOf(name + '=') == -1)
    { return ''; }      // 获取链接中参数部分    
    var queryString = location.href.substring(location.href.indexOf("?") + 1);
    // 分离参数对 ?key=value&key2=value2    
    var parameters = queryString.split("&");
    var pos, paraName, paraValue;
    for (var i = 0; i < parameters.length; i++) {         // 获取等号位置        
        pos = parameters[i].indexOf('=');
        if (pos == -1) { continue; }
        // 获取name 和 value       
        paraName = parameters[i].substring(0, pos);
        paraValue = parameters[i].substring(pos + 1);
        // 如果查询的name等于当前name，就返回当前值，同时，将链接中的+号还原成空格
        if (paraName == name) {
            return unescape(paraValue.replace(/\+/g, " "));
        }
    } return '';
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
////   case 0:
//            colorname = "#FF8080";
//            break;
//        case 1:
//            colorname = "#80FF80";
//            break;
//        case 2:
//            colorname = "#80FFFF";
//            break;
//        case 3:
//            colorname = "#0080FF";
//            break;
//        case 4:
//            colorname = "#FF80FF";
//            break;
//        case 5:
//            colorname = "#FF0000";
//            break;
//        case 6:
//            colorname = "#800000";
//            break;
//        case 7:
//            colorname = "#008040";
//            break;
//        case 8:
//            colorname = "#8000FF";
//            break;
//        case 9:
//            colorname = "#000000";
//            break;
//        case 10:
//            colorname = "#004000";
//            break;
//        case 11:
//            colorname = "#408080";
//            break;
//        case 12:
//            colorname = "#0B9B6C";
//            break;
//        default:
//            colorname = "#0B9BFF";