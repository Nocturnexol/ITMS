/// <reference path="publicjs/jquery-1.4.1-vsdoc.js" />
var tblmaps;
var points; //地图上默认显示的站点位置
var pointType;
var polyline;
var mappolyline; //查询地图的轨迹
var mapEventListener;
var arryPoint; //地图画出来的轨迹点
$(document).ready(function () {
    $("#hideResult").click(function () {
        if ($("#left").css("display") == "block") {
            var width = $(window).width() - 11; //document.documentElement.clientWidth - 10.5;
            var height = $(window).height() - 130; //document.documentElement.clientHeight - 125;
            $("#left").css("display", "none");

            $("#mapTitle").removeClass("title").addClass("title2");
            $("#hideResult").removeClass("hideResult").addClass("hideResult2");
            maplet.resize(width, height);
            maplet.refresh();
        }
        else {
            var width = $(window).width() - 12; //document.documentElement.clientWidth - 10.5;
            var height = $(window).height() - 130; //document.documentElement.clientHeight - 125;
            $("#left").css("display", "block");

            $("#mapTitle").removeClass("title2").addClass("title");
            $("#hideResult").removeClass("hideResult2").addClass("hideResult");

            //减去固定的宽度
            width = width - 344;
            maplet.resize(width, height);
            maplet.refresh();
        }
    });
});
//添加地图点
function BookMarkPoint(p_type) {
    if (maplet.getZoomLevel() < 13) {
        maplet.setZoomLevel(13);
    }
    if (p_type == "Locus") {
        maplet.setMode('drawline', drawline);
    }
    if (p_type == "center") {
        pointType = "center";
        maplet.setMode('bookmark', PointAdd);
    }
    else if (p_type == "Station") {
        pointType = "Station";
        maplet.setMode('bookmark', PointAdd);
    }
}
//划线函数
function drawline(mapdraw) {
    var linepoints = mapdraw.points
    var brush = new MBrush();
    brush.arrow = false;
    brush.stroke = 4;
    brush.fill = false;
    brush.color = 'blue';
    brush.bgcolor = 'red';
    polyline = new MPolyline(linepoints, brush);
    maplet.addOverlay(polyline);
    polyline.setEditable(true);
}
function SavePolyline() {
    var slRoadName = $("#slRoadName").val();
    var linecenter = $("#linecenter").val();
    var ToDirection = $("#slToDirection").val();
    //保存线路轨迹时候判断当前线路轨迹已经保存过
    if (getRoadLineIsExist(slRoadName, ToDirection)) {
        if (!confirm("当前线路轨迹已经存，是否覆盖？")) {
            return;
        }
    }
    if (slRoadName == "-请选择-" || ToDirection == "-请选择-") {
        alert("请选择线路信息！");
        return;
    }
    if (polyline == null || polyline == undefined || linecenter == "") {
        alert("请设置轨迹和中心位置！");
        return;
    }
    var linelatlon = null;
    var arryPoints = polyline.pts;
    $(arryPoints).each(function (i) {
        var point = arryPoints[i];
        if (linelatlon != null) {
            linelatlon += ";" + point.getPid();
        }
        else {
            linelatlon = point.getPid();
        }
    });
    var savejson = '{roadline:"' + slRoadName + '",linelatlon:"' + linelatlon + '",linecenter:"' + linecenter + '",ToDirection:"' + ToDirection + '"}';
    var rqurl = '/BusMap/MapEdit/saveRoadLineLatLon';
    var callbackdata = Jquery_Ajax(savejson, rqurl, false);
    var Jsonstr = eval('(' + callbackdata + ')');
    window.alert(Jsonstr.d);
}
//保存站点信息
function SaveStopPoint() {
    try {
        var StopId = $("#txtStopId").val();
        var strLatlon = $("#txtStrLatlon").val();
        var savejson = '{StopId:"' + StopId + '",strLatlon:"' + strLatlon + '"}';
        var rqurl = '/BusMap/MapEdit/saveStopLatlon';
        var callbackdata = Jquery_Ajax(savejson, rqurl, false);
        //    var Jsonstr = eval('(' + callbackdata + ')');
        //   window.alert(Jsonstr.d);
        window.alert(callbackdata);
    } catch (ex) {

    }
}
function SavePoint() {
    var json = '[';
    $("#TblMap tr").each(function (i) {
        var MapId = $(this).get(0).cells.item(0).getElementsByTagName("input");
        if (MapId.length > 0) {
            if (i == 1) {
                json += "{LevelId:" + MapId[0].value + ",";
            } else {
                json += ",{LevelId:" + MapId[0].value + ",";
            }
            var strlatlon = $(this).get(0).cells.item(1).getElementsByTagName("input")[0].value;
            var StationName = $(this).get(0).cells.item(1).innerText;
            json += "StationName:'" + StationName + "',Strlatlon:'" + strlatlon + "'}";
        }
    });
    json += ']';
    var items = eval(json)
    tblmaps = items;
    if (items.length < 1) {
        window.alert("没有数据不能保存！");
        return;
    }
    if ($("#slToDirection").val() == "-请选择-") {
        window.alert("请选择开往！");
        return;
    }
    if (confirm("要保存当前数据吗？")) {
        var slRoadName = $("#slRoadName").val();
        var linecenter = $("#linecenter").val();
        var ToDirection = $("#slToDirection").val();
        //保存线路轨迹时候判断当前线路轨迹已经保存过
        if (getRoadLineIsExist(slRoadName, ToDirection)) {
            if (!confirm("当前线路轨迹已经存，是否覆盖？")) {
                return;
            }
        }
        var linelatlon = null;
        var arryPoints = mappolyline.pts;
        $(arryPoints).each(function (i) {
            var point = arryPoints[i];
            if (linelatlon != null) {
                linelatlon += ";" + point.getPid();
            }
            else {
                linelatlon = point.getPid();
            }
        });
        var savejson = '{roadline:"' + slRoadName + '",points:' + json + ',linelatlon:"' + linelatlon + '",linecenter:"' + linecenter + '",ToDirection:"' + ToDirection + '"}';
        var rqurl = '/BusMap/MapEdit/saveRoadLine';
        var callbackdata = Jquery_Ajax(savejson, rqurl, false);
        // var Jsonstr = eval('(' + callbackdata + ')');
        // window.alert(Jsonstr.d);
        window.alert(callbackdata);
    }
}
function setmovepoint() {
    maplet.setMode("");
    points = maplet.getMarkers();
    if (points != null) {
        for (var i = 0; i < points.length; i++) {
            points[i].bEditable = true;
        }
    }
}
function setmovemap() {
    maplet.setMode("pan");
    points = maplet.getMarkers();
    if (points != null) {
        for (var i = 0; i < points.length; i++) {
            points[i].bEditable = false;
        }
    }
}
function PointAdd(strUrl) {
    if (strUrl.action == "add") {
        avBubble.width = 280;
        avBubble.height = 240;
        // maplet.centerAndZoom(new MPoint(strUrl.point.lat, strUrl.point.lon));
        //标注一个点
        var new_shado = new MIconShadow("/Resource/Images/icons/shadow.png", 27, 34, -14)
        var marker;
        if (pointType == "center") {
            marker = new MMarker(new MPoint(strUrl.point.lon, strUrl.point.lat), new MIcon("/Resource/Images/icons/pushpin-blue.png", 24, 24));
            $("#linecenter").attr("value", strUrl.point.pid);
            //marker.setLabel(new MLabel((maplet.getMarkers().length + 1) + ""), true);
            marker.setLabel(new MLabel("中心位置"), true);
        }
        else {
            marker = new MMarker(new MPoint(strUrl.point.lon, strUrl.point.lat), new MIcon("/Resource/Images/icons/红.png", 24, 24));
            marker.setContextMenu(menu);
            var strdiv = $("#divcontent").html();
            marker.info = new MInfoWindow("关联信息", strdiv);
        }
        marker.setShadow(new_shado, true)
        maplet.addOverlay(marker);
        marker.openInfoWindow();
        $("#txtStrLatlon").val(strUrl.point.pid);
        maplet.setMode("pan");
        return true;
    }
}
//获得线路是否已经存在？
function getRoadLineIsExist(RoadLine, ToDirection) {
    var IsExist = false;
    var searchjson = '{RoadLine:"' + RoadLine + '",ToDirection:"' + ToDirection + '"}';
    var rqurl = '/BusMap/MapEdit/getRoadLineIsExist';
    var callbackdata = Jquery_Ajax(searchjson, rqurl, false);
    //var jsondata = eval('(' + callbackdata + ')');
    //IsExist = jsondata.d
    IsExist = callbackdata;
    return IsExist;
}

function SearchStationLevel() {
    $('#rdMoveMap').click();
    avBubble.width = 300;
    avBubble.height = 160;
    // $("#sslk").attr("checked", false);
    var TotalDiv = ""; //统计数据
    var RoadLine = $("#slRoadName").val();
    var ToDirection = $("#slToDirection").val();
    if (RoadLine == "-请选择-" || RoadLine == "-请选择-") {
        window.alert("请选择路线名 和开往！");
        return false;
    }
    points = new Array();

    var jsondata = '{RoadLine:"' + RoadLine + '",ToDirection:"' + ToDirection + '"}';
    var rqurl = '/BusMap/MapEdit/getRoadLineListPoint';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var Jsonstr = eval('(' + callbackdata + ')');
    var items = Jsonstr;
    if (items == null) {
        alert('当前路线不存在！');
        return false;
    }
    if ($("#left").css("display") != "block") {
        $("#hideResult").click();
    }
    removeAllOverlays();
    var tbl_temp = "";
    for (var k = 0; k < items.length; k++) {
        var item = items[k];
        $("#result").hide();
        /*---请求地图数据end ---*/
        /*---将得到的地图数据放入表格中---*//// <reference path="..//Images/TabBak.png" />
        /// <reference path="..//Images/minus.png" />
        if (k == 0) {
            tbl_temp += "<div style='cursor:pointer;'><table style='border-style: solid; border-width: 1px;border-color: #CCCCCC #999 #999 #CCCCCC;width:100%;text-align:left' "
                        + "onclick='javascript:{ if ($(\"#result" + k + "\").css(\"display\") == \"block\") {$(\"#result" + k + "\").hide();$(\"#img" + k
                        + "\").attr(\"src\",\"/Resource/Images/plus.png\");}else{$(\"#result" + k + "\").show();$(\"#img" + k
                        + "\").attr(\"src\",\"/Resource/Images/minus.png\");}};'><tr><th><img style='border:0px' src='/Resource/Images/plus.png' id='img" + k
                        + "' /></th><td>" + item.RoadLineInfo + "(" + item.TicketStyle + ")</td></tr></table></div><div style='font-size:12px; display:block;' id='result" + k
                        + "'><table style='font-size:12px;height:100%' Id='TblMap'>"
        }
        else {
            tbl_temp += "<div style='cursor:pointer;'><table style='border-style: solid; border-width: 1px;border-color: #CCCCCC #999 #999 #CCCCCC;width:100%;text-align:left' "
                        + "onclick='javascript:{ if ($(\"#result" + k + "\").css(\"display\") == \"block\") {$(\"#result" + k + "\").hide();$(\"#img" + k
                        + "\").attr(\"src\",\"/Resource/Images/plus.png\");}else{$(\"#result" + k + "\").show();$(\"#img" + k
                        + "\").attr(\"src\",\"/Resource/Images/minus.png\");}};'><tr><th><img style='border:0px' src='/Resource/Images/plus.png' id='img" + k
                        + "' /></th><td>" + item.RoadLineInfo + "(" + item.TicketStyle + ")</td></tr></table></div><div style='font-size:12px; display:none;' id='result" + k
                        + "'><table style='font-size:12px;height:100%;' Id='TblMap'>"
        }
        for (var j = 0; j < item.StationPointInfos.length; j++) {
            var strLatLon = item.StationPointInfos[j].StrLatlon;
            var labl = item.StationPointInfos[j].PointName; //标签点的名称
            var SETime = item.StationPointInfos[j].SETime; //首末班车时间
            if (j == item.StationPointInfos.length - 1 && k == items.length - 1) {
                var RoadNum = item.RoadNum; //路线总数
                var BrandNum = item.BrandNum; //站牌总数
                TotalDiv = "<div><table style='height:40px; float: left; margin-left: 8px;'><td>路线数：" + RoadNum + "</td><td> &nbsp;&nbsp;&nbsp;&nbsp;站牌数：" + BrandNum + "</td></table></div>";
            }
            if (strLatLon != "") {
                var lat = item.StationPointInfos[j].StrLatlon.split(',')[0]; //经度
                var lon = item.StationPointInfos[j].StrLatlon.split(',')[1]; //纬度
                var point = new MPoint(lat, lon); //坐标
                marker = new MMarker(point, new MIcon("/Resource/Images/icons/1/icon_03.png", 16, 16));
                marker.setLabel(new MLabel(labl), true); //添加标签
                marker.label.setVisible(false)//隐藏标签
                marker.setContextMenu(menu);
                var lineList = item.StationPointInfos[j].LineList.split("、");
                var strlineList = "";
                for (var i = 0; i < lineList.length; i++) {
                    if (strlineList != "") {
                        strlineList += "、" + lineList[i];

                    } else {
                        strlineList += lineList[i];
                    }
                }
                var str_div = "<div style='font-size:12px;'><table><tr><td>站点站址</td><td>" + item.StationPointInfos[j].Address + "</td></tr><tr><td>停靠线路</td><td>" + strlineList + "</td></tr><tr><td>站点编号</td><td><a href='javascript:;' onclick='javascript:openimage(\"" + item.StationPointInfos[j].StopId + "\")'>" + item.StationPointInfos[j].StopId + "</a></td></tr><tr><td>站点照片</td><td><a href='javascript:;' onclick='javascript:openimage(\"" + item.StationPointInfos[j].StopId + "\")'>点击查看照片</a></td></tr></table></div>";
                marker.info = new MInfoWindow("当前站点:" + labl, str_div, "20px", "20px");
                if (j == 0) {
                    tbl_temp += "<tr><td >站级</td><td  style='text-align:center;width:100px'>站名</td><td style='width:80px;'>首末班时间</td><td style='width:60px;'>站点编号</td></tr>";
                    tbl_temp += "<tr><td ><input type='text' id='txtmap" + (j + 1) + "' value='" + (j + 1) + "' style='width:20px;' />.</td><td  style='color:Blue'><a href='javascript:LatLonToCenter(" + strLatLon + ",true)'>" + labl + "</a><input type='hidden'  value='" + strLatLon + "'></td><td >" + SETime + "</td><td style='color:Blue'>" + item.StationPointInfos[j].StopId + "</td></tr>";
                }
                else {
                    tbl_temp += "<tr><td ><input type='text' id='txtmap" + (j + 1) + "' value='" + (j + 1) + "' style='width:20px;' />.</td><td  style='color:Blue'><a href='javascript:LatLonToCenter(" + strLatLon + ",true)'>" + labl + "</a><input type='hidden'  value='" + strLatLon + "'></td><td >" + SETime + "</td><td style='color:Blue'>" + item.StationPointInfos[j].StopId + "</td></tr>";
                }
                MEvent.addListener(marker, "click", function (omarker) {
                    //omarker.pt.getPid();
                    $("#txtStrLatlon").val(omarker.pt.getPid());
                    omarker.openInfoWindow();
                });
                MEvent.addListener(marker, "mouseover", function (mk) {
                    if (mk.label) mk.label.setVisible(true);
                })
                MEvent.addListener(marker, "mouseout", function (mk) {
                    if (mk.label) mk.label.setVisible(false);
                })
                MEvent.addListener(marker, "drag", function (mk) {
                    //omarker.pt.getPid();
                    // window.alert(mk.pt.pid);
                    var newstrlatlon = mk.pt.pid;
                    var strStationName = mk.label.label;
                    $("#TblMap tr").each(function (i) {
                        var MapId = $(this).get(0).cells.item(0).getElementsByTagName("input");
                        if (MapId.length > 0) {
                            var StationName = $(this).get(0).cells.item(1).innerText;
                            if (strStationName == StationName) {
                                $(this).get(0).cells.item(1).getElementsByTagName("input")[0].value = newstrlatlon;
                            }
                        }
                    });
                });
                points.push(marker);
                if (k == 0) {
                    maplet.addOverlay(marker);
                }
            }
            else {
                if (j == 0) {
                    tbl_temp += "<tr><td>站级</td><td>站名</td><td>首末班时间</td></tr>";
                    tbl_temp += "<tr><td><input type='text' id='txtmap" + (j + 1) + "' value='" + (j + 1) + "' style='width:20px;'/>.</td><td style='color:Green'>" + labl + " <input type='hidden'  value=''></td><td >" + SETime + ".</td><td style='color:Blue'>" + item.StationPointInfos[j].StopId + "</td></tr>";
                }
                else {
                    tbl_temp += "<tr><td><input type='text' id='txtmap" + (j + 1) + "' value='" + (j + 1) + "' style='width:20px;'/>.</td><td style='color:Green'>" + labl + " <input type='hidden'  value=''></td><td >" + SETime + ".</td><td style='color:Blue'>" + item.StationPointInfos[j].StopId + "</td></tr>";
                }
            }

        }
        tbl_temp += "</table></div>";
        //将所有经纬度添加到集合中去
        //用于存放当前点的集合的数组
        if (item.LineLatLon != null && item.LineLatLon != "" && item.LineLatLon != undefined) {
            var strlinelatlon = "";
            var points_temp = new Array();
            for (var i = 0; i < item.LineLatLon.length; i++) {

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

            //创建笔刷将所有的点连接起来
            var brush = new MBrush();
            brush.arrow = false;
            brush.stroke = 4;
            brush.fill = false;
            brush.color = 'blue';
            brush.bgcolor = 'red';
            mappolyline = new MPolyline(points_temp, brush);
            mappolyline.setEditable(true);
            maplet.addOverlay(mappolyline);
        }
        //将地图移动到中心位置
        if (item.CenterPoint != null) {

            var lat = item.CenterPoint.split(',')[0];
            var lon = item.CenterPoint.split(',')[1]
            maplet.centerAndZoom(new MPoint(lat, lon), 10);
            $("#linecenter").attr("value", item.CenterPoint);
        }

        tbl_temp = TotalDiv + tbl_temp;
    }
    $("#resultcontent").html(tbl_temp);
    //显示站点名字
    ShowStationName();
}
function SearchPoint() {
    $('#rdMoveMap').click();
    removeAllOverlays();
    var city = $("#sltCity").val();
    if (indexcity.indexOf(city) == -1) {
        window.alert("超出查询范围，如果需要查询该区域地图请与供应商联系！");
        return false;
    }
    var keyword = $("#txtKeyWord").val();
    //window.alert();
    var roadline_search = $("#bsmap_head_tab_bus").attr("checked"); //线路查询
    var stationadd_search = $("#bsmap_head_tab_nav").attr("checked"); //站址查询
    var address_search = $("#bsmap_head_tab_nb").attr("checked"); //地址查询

    if (roadline_search) {
        var direction = $("#direction_s").attr("checked") == true ? "上行" : "下行";
        MapSearchByRoadLine(city, keyword, direction);
        return true;
    }
    else {
        var type_name = stationadd_search == true ? "公交车站" : "";
        var jsondata = '{city:"' + city + '",keyword:"' + keyword + '",searchtype:"' + type_name + '"}';
        var rqurl = 'WebService.asmx/getPoiByKeyword';
        var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
        var Jsonstr = eval('(' + callbackdata + ')');
        var item = Jsonstr.d;
        if (item == null) {
            alert("未查询到任何相关信息！");
            return false;
        }
        if ($("#left").css("display") != "block") {
            $("#hideResult").click();
        }
        var markers = new Array();
        var tbl_temp = "<table style='font-size:12px;top:20px;position:absolute;'>"
        for (var i = 0; i < item.length; i++) {
            var strLatLon = item[i].StrLatlon;
            var lon = item[i].StrLatlon.split(',')[0];
            var let = item[i].StrLatlon.split(',')[1];
            var labl = item[i].PointName;
            var new_shado = new MIconShadow("/Resource/Images/icons/shadow.png", 27, 34, -14)
            marker = new MMarker(new MPoint(lon, let), new MIcon("/Resource/Images/icons/绿.png", 24, 24));
            marker.setLabel(new MLabel(labl), false);
            marker.label.setVisible(false)
            marker.setContextMenu(menu);
            marker.setShadow(new_shado, true)
            ////
            var str_div = labl + "<br/>" + item[i].Address;
            marker.info = new MInfoWindow("当前站点" + labl, str_div);
            tbl_temp += "<tr><td>" + (i + 1) + ".</td><td  style='color:Blue'><a href='javascript:LatLonToCenter(" + strLatLon + ",false)'>" + str_div + "</a></td></tr>"
            MEvent.addListener(marker, "click", function (omarker) {
                document.getElementById("txtStrlatlon").value = omarker.pt.getPid();
                omarker.openInfoWindow();
            });
            MEvent.addListener(marker, "mouseover", function (mk) {
                if (mk.label) mk.label.setVisible(true);
            })
            MEvent.addListener(marker, "mouseout", function (mk) {
                if (mk.label) mk.label.setVisible(false);
            });
            MEvent.addListener(marker, "drag", function (mk) {
                //window.alert(mk.pt.getPid());
            });
            markers[i] = marker;
        }
        tbl_temp += "</table>"
        $("#resultcontent").html(tbl_temp);
        for (var j = 0; j < markers.length; j++) {
            maplet.addOverlay(markers[j]);
        }
        if (markers.length > 0) {
            maplet.centerAndZoom(new MPoint(markers[markers.length - 1].pt.getPid()), 12);
        }
    }

}
function edit() {
    document.getElementById("divsee").style.display = "none";
    document.getElementById("divedit").style.display = "block";
    document.getElementById("esc").style.display = "block";
    document.getElementById("edit").style.display = "none";
}
function see() {
    document.getElementById("divedit").style.display = "none";
    document.getElementById("divsee").style.display = "block";
    document.getElementById("esc").style.display = "none";
    document.getElementById("edit").style.display = "block";

}
function MapSearchByRoadLine(city, keyword) {
    $('#rdMoveMap').click();
    //设置信息窗口的宽和高
    avBubble.width = 300;
    avBubble.height = 100;
    /*---请求得到地图数据start---*/
    var jsondata = '{city:"' + city + '",keyword:"' + keyword + '"}';
    var rqurl = 'WebService.asmx/getRodNameListByKeyword';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var Jsonstr = eval('(' + callbackdata + ')');
    var item = Jsonstr.d;
    if (item == null) {
        alert("未查询到任何相关信息！");
        return;
    }
    if ($("#left").css("display") != "block") {
        $("#hideResult").click();
    }
    var tbl_temp = "<div style='font-size:12px;top:10px;position:absolute;'></div>";
    tbl_temp += "<table style='font-size:12px;top:20px;position:absolute;' Id='TblMap'>"
    if (item.length > 0) {
        tbl_temp += "<tr><th>站名<th></tr>"
        for (var i = 0; i < item.length; i++) {
            tbl_temp += "<tr><td><a href='javascript:MapSearchByRoad(\"" + city + "\",\"" + item[i] + "\");'>" + item[i] + "</a><td></tr>"
        }
    }
    else {
        tbl_temp += "<tr><th>当前线路不存在！<th></tr>"
    }
    tbl_temp += "</table>";
    $("#resultcontent").html(tbl_temp);

}
//地图查询
function MapSearchByRoad(city, keyword) {
    $('#rdMoveMap').click();
    points = new Array()
    var jsondata = '{city:"' + city + '",keyword:"' + keyword + '"}';
    var rqurl = 'WebService.asmx/getRodNameByKeyword';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var Jsonstr = eval('(' + callbackdata + ')');
    var item = Jsonstr.d;
    if (item == null) {
        alert("未查询到任何相关信息！");
        return;
    }
    if ($("#left").css("display") != "block") {
        $("#hideResult").click();
    }
    /*---请求地图数据end ---*/
    /*---将得到的地图数据放入表格中---*/
    var tbl_temp = "<div style='font-size:12px;top:10px;position:absolute;'>" + item.StationPointInfos[0].Address + "&nbsp;<div><table style='font-size:12px;top:20px;position:absolute;' Id='TblMap'>"
    for (var j = 0; j < item.StationPointInfos.length; j++) {
        var strLatLon = item.StationPointInfos[j].StrLatlon;
        var lat = item.StationPointInfos[j].StrLatlon.split(',')[0]; //经度
        var lon = item.StationPointInfos[j].StrLatlon.split(',')[1]; //纬度
        var labl = item.StationPointInfos[j].PointName; //标签点的名称
        var point = new MPoint(lat, lon); //坐标点
        if (j == 0) {
            marker = new MMarker(point, new MIcon("/Resource/Images/icons/起点.png", 24, 24));
        }
        else if (j == (item.StationPointInfos.length - 1)) {
            marker = new MMarker(point, new MIcon("/Resource/Images/icons/终点.png", 24, 24));
        }
        else {
            marker = new MMarker(point, new MIcon("/Resource/Images/icons/1/icon_03.png", 16, 16));
        }
        marker.setLabel(new MLabel(labl), true); //添加标签
        marker.label.setVisible(false)//隐藏标签
        points.push(marker);
        marker.setContextMenu(menu);
        ////
        var str_div = "<div style='font-size:12px;'>" + labl + " <br/>" + item.StationPointInfos[j].Address + "</div>";
        marker.info = new MInfoWindow("当前站点:" + labl, str_div, "100px", "100px");
        if (j == 0) {
            // tbl_temp += "<tr><th colspan='2'>" + item.StationPointInfos[j].Address + "&nbsp;<a id='ReturnLine' href='javascript:void(0);' onclick='RoadSearch()'>返程线路</a></th></tr>";
            tbl_temp += "<tr><th>编号</th><th>站名</th></tr>";
            tbl_temp += "<tr><td ><input type='text' id='txtmap" + (j + 1) + "' value='" + (j + 1) + "' style='width:20px;'/>.</td><td  style='color:Blue'><a href='javascript:LatLonToCenter(" + strLatLon + ",true)'>" + labl + "</a><input type='hidden'  value='" + strLatLon + "'></td></tr>";
        }
        else {
            tbl_temp += "<tr><td ><input type='text' id='txtmap" + (j + 1) + "' value='" + (j + 1) + "' style='width:20px;'/>.</td><td  style='color:Blue;'><a href='javascript:LatLonToCenter(" + strLatLon + ",true)'>" + labl + "</a><input type='hidden'  value='" + strLatLon + "'></td></tr>";
        }
        // marker.bEditable = true;
        // 添加对marker 的事件
        MEvent.addListener(marker, "click", function (omarker) {
            //omarker.pt.getPid();
            $("#txtStrLatlon").val(omarker.pt.getPid());
            omarker.openInfoWindow();
        });
        MEvent.addListener(marker, "mouseover", function (mk) {
            if (mk.label) mk.label.setVisible(true);
        })
        MEvent.addListener(marker, "mouseout", function (mk) {
            if (mk.label) mk.label.setVisible(false);
        })
        MEvent.addListener(marker, "drag", function (mk) {
            //omarker.pt.getPid();
            // window.alert(mk.pt.pid);
            var newstrlatlon = mk.pt.pid;
            var strStationName = mk.label.label;
            $("#TblMap tr").each(function (i) {
                var MapId = $(this).get(0).cells.item(0).getElementsByTagName("input");
                if (MapId.length > 0) {
                    var StationName = $(this).get(0).cells.item(1).innerText;
                    if (strStationName == StationName) {
                        $(this).get(0).cells.item(1).getElementsByTagName("input")[0].value = newstrlatlon;
                    }
                }
            });

        });
        if ($("#isshow").attr("checked") == false) {
            maplet.addOverlay(marker);
        }
    }
    tbl_temp += "</table>";
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
    brush.color = 'blue';
    brush.bgcolor = 'red';
    mappolyline = new MPolyline(points_temp, brush);
    polyline = mappolyline;
    mappolyline.setEditable(true);
    maplet.addOverlay(mappolyline);
    $("#resultcontent").html(tbl_temp);
    $("#roadlines").val(strlinelatlon);
}
//设置鼠标样式
function SetCursorStyle() {
    //maplet.setCursorStyle("default","/Resource/Images/larrow.cur");
    //maplet.setCursorStyle("pointer","/Resource/Images/lnodrop.cur");
    //maplet.setCursorStyle("move","/Resource/Images/lmove.cur");
    //maplet.setCursorStyle("crosshair","/Resource/Images/lwait.cur");
}
//显示站点名称
function ShowStationName() {
    if (points != null) {
        for (var i = 0; i < points.length; i++) {
            var omk = points[i];
            if ($("#sslk").attr("checked")) {
                omk.label.setVisible(true);
            }
            else {
                omk.label.setVisible(false);
            }
        }
    }
}
//下拉级联初始化
function selectitem(item) {
    var itemtext = item.options[item.selectedIndex].text;
    var c_id = item.id;
    if (c_id == "slRoadName") {
        $("#slToDirection").empty();
        if (itemtext != "-请选择-") {
            var options = "<option value='-请选择-'>-请选择-</option>";
            var str_where = "RoadLine='" + itemtext + "'";
            var jsondata = '{table_name:"tblRoadBaseInfo",str_field:"ToDirection",str_where:"' + str_where + '"}';
            var rqurl = '/BusMap/MapEdit/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            if (Jsonstr == null)
                return false;
            for (var i = 0; i < Jsonstr.length; i++) {
                options += "<option value=" + Jsonstr[i] + ">" + Jsonstr[i] + "</option>";
            }
            $(options).appendTo($("#slToDirection")); //
        }
    }
    else if (c_id == "Area") {
        var District = $(item).parent().parent().parent().find("select[id='District']")
        $(District).empty();
        var RoadName = $(item).parent().parent().parent().find("select[id='RoadName']")
        $(RoadName).empty();
        var StationName = $(item).parent().parent().parent().find("select[id='StationName']")
        $(StationName).empty();
        var PathDirection = $(item).parent().parent().parent().find("select[id='PathDirection']")
        $(PathDirection).empty();
        var StationAddress = $(item).parent().parent().parent().find("select[id='StationAddress']")
        $(StationAddress).empty();
        if (itemtext != "-请选择-") {
            var options = "<option value='-请选择-'>-请选择-</option>";
            var str_where = "Area='" + itemtext + "'";
            var jsondata = '{table_name:"tblStop",str_field:"District",str_where:"' + str_where + '"}';
            var rqurl = '/BusMap/MapEdit/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            // var item = Jsonstr.d;
            if (Jsonstr == null) {
                return false;
            }
            for (var i = 0; i < Jsonstr.length; i++) {
                options += "<option value=" + Jsonstr[i] + ">" + Jsonstr[i] + "</option>";
            }
            $(options).appendTo($(District)); //
        }
    }
    else if (c_id == "District") {
        var Area = $(item).parent().parent().parent().find("select[id='Area']")

        var RoadName = $(item).parent().parent().parent().find("select[id='RoadName']")
        $(RoadName).empty();
        var StationName = $(item).parent().parent().parent().find("select[id='StationName']")
        $(StationName).empty();
        var PathDirection = $(item).parent().parent().parent().find("select[id='PathDirection']")
        $(PathDirection).empty();
        var StationAddress = $(item).parent().parent().parent().find("select[id='StationAddress']")
        $(StationAddress).empty();
        if (itemtext != "-请选择-") {
            var options = "<option value='-请选择-'>-请选择-</option>";
            var str_where = "Area='" + $(Area).val() + "' and District='" + itemtext + "'";
            var jsondata = '{table_name:"tblStop",str_field:"RoadName",str_where:"' + str_where + '"}';
            var rqurl = '/BusMap/MapEdit/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            // var item = Jsonstr.d;
            if (Jsonstr == null) {
                return false;
            }
            for (var i = 0; i < Jsonstr.length; i++) {
                options += "<option value=" + Jsonstr[i] + ">" + Jsonstr[i] + "</option>";
            }
            $(options).appendTo($(RoadName)); //
        }
    }
    else if (c_id == "RoadName") {
        var Area = $(item).parent().parent().parent().find("select[id='Area']");
        var District = $(item).parent().parent().parent().find("select[id='District']");
        var StationName = $(item).parent().parent().parent().find("select[id='StationName']");
        $(StationName).empty();
        var PathDirection = $(item).parent().parent().parent().find("select[id='PathDirection']")
        $(PathDirection).empty();
        var StationAddress = $(item).parent().parent().parent().find("select[id='StationAddress']")
        $(StationAddress).empty();
        if (itemtext != "-请选择-") {
            var options = "<option value='-请选择-'>-请选择-</option>";
            var str_where = "Area='" + $(Area).val() + "' and District='" + $(District).val() + "' and RoadName='" + itemtext + "'";
            var jsondata = '{table_name:"tblStop",str_field:"StationName",str_where:"' + str_where + '"}';
            var rqurl = '/BusMap/MapEdit/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            //  var item = Jsonstr.d;
            if (Jsonstr == null) {
                return false;
            }
            for (var i = 0; i < Jsonstr.length; i++) {
                options += "<option value=" + Jsonstr[i] + ">" + Jsonstr[i] + "</option>";
            }
            $(options).appendTo($(StationName)); //
        }
    }
    else if (c_id == "StationName") {
        var Area = $(item).parent().parent().parent().find("select[id='Area']");
        var District = $(item).parent().parent().parent().find("select[id='District']");
        var RoadName = $(item).parent().parent().parent().find("select[id='RoadName']");

        var PathDirection = $(item).parent().parent().parent().find("select[id='PathDirection']");
        $(PathDirection).empty();
        var StationAddress = $(item).parent().parent().parent().find("select[id='StationAddress']");
        $(StationAddress).empty();
        if (itemtext != "-请选择-") {
            var options = "<option value='-请选择-'>-请选择-</option>";
            var str_where = "Area='" + $(Area).val() + "' and District='" + $(District).val() + "' and RoadName='" + $(RoadName).val() + "' and StationName='" + itemtext + "'";
            var jsondata = '{table_name:"tblStop",str_field:"PathDirection",str_where:"' + str_where + '"}';
            var rqurl = '/BusMap/MapEdit/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            //  var item = Jsonstr.d;
            if (Jsonstr == null) {
                return false;
            }
            for (var i = 0; i < Jsonstr.length; i++) {
                options += "<option value=" + Jsonstr[i] + ">" + Jsonstr[i] + "</option>";
            }
            $(options).appendTo($(PathDirection)); //
        }
    }
    else if (c_id == "PathDirection") {
        var Area = $(item).parent().parent().parent().find("select[id='Area']");
        var District = $(item).parent().parent().parent().find("select[id='District']");
        var RoadName = $(item).parent().parent().parent().find("select[id='RoadName']");
        var StationName = $(item).parent().parent().parent().find("select[id='StationName']");

        var StationAddress = $(item).parent().parent().parent().find("select[id='StationAddress']");

        $(StationAddress).empty();
        if (itemtext != "-请选择-") {
            var options = "<option value='-请选择-'>-请选择-</option>";
            var str_where = "Area='" + $(Area).val() + "' and District='" + $(District).val() + "' and RoadName='" + $(RoadName).val() + "' and StationName='" + $(StationName).val() + "' and PathDirection='" + itemtext + "'";
            var jsondata = '{table_name:"tblStop",str_field:"StationAddress",str_where:"' + str_where + '"}';
            var rqurl = '/BusMap/MapEdit/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            //   var item = Jsonstr.d;
            if (Jsonstr == null) {
                return false;
            }
            for (var i = 0; i < Jsonstr.length; i++) {
                options += "<option value=" + Jsonstr[i] + ">" + Jsonstr[i] + "</option>";
            }
            $(options).appendTo($(StationAddress)); //
        }
    }
    else if (c_id == "StationAddress") {
        var Area = $(item).parent().parent().parent().find("select[id='Area']");
        var District = $(item).parent().parent().parent().find("select[id='District']");
        var RoadName = $(item).parent().parent().parent().find("select[id='RoadName']");
        var StationName = $(item).parent().parent().parent().find("select[id='StationName']");
        var PathDirection = $(item).parent().parent().parent().find("select[id='PathDirection']");
        var txtStopId = $(item).parent().parent().parent().find("input[id='txtStopId']")
        if (itemtext != "-请选择-") {
            var options = "<option value='-请选择-'>-请选择-</option>";
            var str_where = "Area='" + $(Area).val() + "' and District='" + $(District).val() + "' and RoadName='" + $(RoadName).val() + "' and StationName='" + $(StationName).val() + "'and PathDirection='" + $(PathDirection).val() + "' and StationAddress='" + itemtext + "'";
            var jsondata = '{table_name:"tblStop",str_field:"StopId",str_where:"' + str_where + '"}';
            var rqurl = '/BusMap/MapEdit/GetFieldByWhere';
            var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
            var Jsonstr = eval('(' + callbackdata + ')');
            //   var item = Jsonstr.d;
            if (Jsonstr == null) {
                return false;
            }
            for (var i = 0; i < Jsonstr.length; i++) {
                $(txtStopId).val(Jsonstr[i]);
                tempStopIdVal = Jsonstr[i];
            }
        }
    }
}
var tempStopIdVal = "";
function StopIdBlur(item) {

    var Area = $(item).parent().parent().parent().find("select[id='Area']");
    var District = $(item).parent().parent().parent().find("select[id='District']");
    var RoadName = $(item).parent().parent().parent().find("select[id='RoadName']");
    var StationName = $(item).parent().parent().parent().find("select[id='StationName']");
    var PathDirection = $(item).parent().parent().parent().find("select[id='PathDirection']");
    var StationAddress = $(item).parent().parent().parent().find("select[id='StationAddress']");
    var txtStopId = $(item).val();
    if (tempStopIdVal != txtStopId && txtStopId != "") {
        $(District).empty();
        $(RoadName).empty();
        $(StationName).empty();
        $(PathDirection).empty();
        $(StationAddress).empty();


        $.ajax({
            url: "/BusPoles/PolesNewList/StopIdChange",
            type: "GET",
            dataType: "json",
            data: { StopId: txtStopId },
            success: function (data) {

                if (data.length == 0) {
                    layer.alert("站点编号" + txtStopId + "不存在", { icon: 0 });
                } else {
                    $(Area).val(data[0].Area);
                    var option = "<option value='" + data[0].District + "'>" + data[0].District + "</option>";
                    $(option).appendTo($(District));
                    option = "<option value='" + data[0].RoadName + "'>" + data[0].RoadName + "</option>";
                    $(option).appendTo($(RoadName));
                    option = "<option value='" + data[0].StationName + "'>" + data[0].StationName + "</option>";
                    $(option).appendTo($(StationName));
                    option = "<option value='" + data[0].PathDirection + "'>" + data[0].PathDirection + "</option>";
                    $(option).appendTo($(PathDirection));
                    option = "<option value='" + data[0].StationAddress + "'>" + data[0].StationAddress + "</option>";
                    $(option).appendTo($(StationAddress));

                }

            }
        });
    }
}