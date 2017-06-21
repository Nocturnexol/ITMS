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

        url: rqurl,
        async: sync,
        type: 'post',
        data: jsondata,
        dataType: 'json',
        timeout: 1000000,
        beforeSend: function (XMLHttpRequest) {
            $(".overmask").show();

        },
        complete: function (XMLHttpRequest, textStatus) {
            $(".overmask").hide();
        },
        error: function (ex) {
            alert('请求数据出错，请刷新页面后重试！');
            return false;
        },
        success: callback
    });
}


var maplet = null; //地图对象
var marker = null; //标注对象
var menu = null;
var indexcity = "上海市";
var apiType = 1; //是否为明文经纬度
var points = []; //结果数据经纬度 MPoint 对象。
var roadLonLats = new Map();
var roadPolyline = new Map();

var levelMap = new Map();
var levelPointMap = new Map();

var menu = new MContextMenu();
function initMap() {
    try {
        maplet = new Maplet("maps");
    }
    catch (ex) {
        window.alert("地图加载失败，请刷新重试！");
        return;
    }
    maplet.centerAndZoom(new MPoint(indexcity), 11); //设置默认城市和缩放级别
    maplet.addControl(new MStandardControl());
    maplet.showLogo(false); //隐藏Logo 等等一些信息
    maplet.showNavLogo(false);
    maplet.clickToCenter = false; //点击了之后是否将点击的位置设置为中心点
    maplet.setAutoZoom(); //设置更具地图自动适应
    //  maplet.customInfoWindow = true;
    maplet.setOverviewLocation({ type: eval("Maplet.RIGHT_BOTTOM") })//设置鹰眼的位置
    var width = $(window).width() - $('#leftPanel1').width(); //document.documentElement.clientWidth - 10.5;
    var height = $(window).height();
    maplet.resize(width, height); //地图初始化时候的大小


    maplet.setContextMenu(menu);
    var mpoint = undefined;
    MEvent.addListener(maplet, "contextmenu", function () {
        mpoint = arguments[0];
    });

    menu.addItem(new MContextMenuItem("设置站点位置", function () {
        BookMarkPoint("Station");
    }));
    menu.addItem(new MContextMenuItem("拉框删除", function () {
        maplet.setMode("lookup", lookupdellonlat);
    }));
    menu.addItem(new MContextMenuItem("站点连线", function () {
        levelPointMap.keys.forEach(function (ii) {
            var levelPoin = levelPointMap.get(ii);
            var points_temp = new Array();
            if (true) {
                var maxLon = -180;
                var maxLat = -180;
                var minLon = 180;
                var minLat = 180;

                var strLonLat = "";

                levelPoin.keys.forEach(function (kk) {
                    if (levelPoin.get(kk) != undefined && levelPoin.get(kk) != null) {
                        var lat = parseFloat(levelPoin.get(kk).pt.lon);
                        var lon = parseFloat(levelPoin.get(kk).pt.lat);

                        if (lat < minLat) {
                            minLat = lat;
                        } if (lat > maxLat) {
                            maxLat = lat;
                        }
                        if (lon < minLon) {
                            minLon = lon;
                        } if (lon > maxLon) {
                            maxLon = lon;
                        }
                        var point = new MPoint(lat, lon);
                        points_temp.push(point);
                        if (strLonLat == null || strLonLat == "") {
                            strLonLat += levelPoin.get(kk).pt.getPid();

                        } else {
                            strLonLat += ";" + levelPoin.get(kk).pt.getPid();
                        }
                    }

                });
                if (ii == 0) {
                    upStrLonLat = strLonLat;
                } else {
                    downStrLonLat = strLonLat;
                }
                //创建笔刷将所有的点连接起来
                var brush = new MBrush();
                brush.arrow = false;
                brush.stroke = 4;
                brush.fill = false;
                if (ii == 0)
                    brush.color = '#416db4';
                else
                    brush.color = '#149714';
                brush.bgcolor = 'red';
                brush.overlap = {
                    // Boolean类型，是否显示边框突出。  
                    enable: true,
                    // Int类型，突出边框宽度。  
                    stroke: 1,
                    // String类型,突出边框颜色值，可以是十六进制数字 HTML 样式或颜色名称，例如，使用 #ff0000 和 red 都可以。  
                    color: "#ffffff",
                    // Int类型,突出边框透明度，取值范围0-100。  
                    transparency: 80
                }
                var mappolyline = new MPolyline(points_temp, brush);
                if (ii == 0) {
                    upPolyline = mappolyline;
                } else {
                    downPolyline = mappolyline;
                }
                mappolyline.setEditable(true);

                maplet.addOverlay(mappolyline);
                MEvent.addListener(maplet, "edit", EditLine);
                if (maxLon != -180 & maxLat != -180 && minLon != 180 & minLat != 180) {
                    maplet.centerAndZoom(new MPoint((maxLat + minLat) / 2, (maxLon + minLon) / 2), 13);
                }

            }

        });
    }));
    menu.addItem(new MContextMenuItem("编辑位置", function (e, a, b) {
        b.bEditable = true;
        maplet.setMode("");
    }));
//    menu.addItem(new MContextMenuItem("删除位置", DelPoint));
    menu.addItem(new MContextMenuItem("显示站名", ShowStationName));
    menu.addItem(new MContextMenuItem("隐藏站名", HideStationName));
    menu.addItem(new MContextMenuItem("保存站点位置", SavePointGPS));

}
function ShowStationName() {
    showstationname(1);
}
function HideStationName() {
    showstationname(0);
}
function showstationname(type) {

    levelPointMap.keys.forEach(function (item) {

        var levelPoints = levelPointMap.get(item);
        levelPoints.keys.forEach(function (item) {

            var omk = levelPoints.get(item);
            if (omk != undefined) {
                if (type == 0) {

                    omk.label.setVisible(false);

                } else {
                    omk.label.setVisible(true);

                }
            }
        });
    });

}

function SavePointGPS(contextMenuItem, contextMenu, overlay) {

    if (overlay != undefined && overlay != null) {
        var stopId = $(overlay.info.content).find("a[_data='stopid']");
        if (stopId.length == 0) {
            layer.alert("尚未设置站点信息关联");
        } else {
        layer.confirm("确定要保存点吗?", { icon: 0 }, function (index) {
            layer.close(index);

            var div = overlay.info.content;
            var StopId = $(div).find("a[_data='stopid']").text();
            var strLatlon = overlay.pt.getPid();
            var savejson = '{StopId:"' + StopId + '",strLatlon:"' + strLatlon + '"}';
            var rqurl = '/BusMap/MapEdit/saveStopLatlon';
            var callbackdata = Jquery_Ajax(savejson, rqurl, false);
//            levelMap.keys.forEach(function (k) {
//                var levelPoints = levelMap.get(k);
//                for (var ii = 0; ii < levelPoints.length; ii++) {
//                    var levelObj = levelPoints[ii];
//                    if (levelObj.StopId == StopId) {
//                        var t = levelPointMap.get(k);
//                       
//                        t.remove(ii);

//                        var point = new MPoint(overlay.pt.lon, overlay.pt.lat); //坐标
//                        var marker = new MMarker(point, new MIcon("/Resource/Images/icons/1/icon_03.png", 16, 16));
//                        marker.setLabel(overlay.label, true); //添加标签
//                        marker.info = overlay.info;
//                        t.set(ii, marker);
//                        maplet.removeOverlay(overlay);
//                        maplet.addOverlay(marker);
//                        levelPointMap.set(k, t);
//                        break;
//                    }

//                }

//            });


            if (callbackdata.indexOf('成功'))
                layer.alert(callbackdata, { icon: 1 });
            else
                layer.alert(callbackdata, { icon: 0 });
        });
        }
    }
}

/*删除站点位置*/
function DelPoint(contextMenuItem, contextMenu, overlay) {
    if (confirm("确定要删除点吗?")) {
       
        overlay.remove(true);
    }
}
function lookupdellonlat(dataObj) {
    var allPoi = maplet.getMarkers();
    for (var i = 0; i < allPoi.length; i++) {
        if (check(allPoi[i].pt, dataObj.min, dataObj.max)) {
            maplet.removeOverlay(allPoi[i]);
        }
    }
}
//保存站点信息
function SaveStopPoint(id) {
    try {
        var div = $(id).parent().parent().parent().parent().parent();
        var StopId = div.find("input[name='txtStopId']").val();
        var strLatlon = div.find("div[_title='lonlat']").text();
        var savejson = '{StopId:"' + StopId + '",strLatlon:"' + strLatlon + '"}';
        var rqurl = '/BusMap/MapEdit/saveStopLatlon';
        
        layer.confirm("确定要保存点吗?", { icon: 0 }, function (index) {
            layer.close(index);
            var callbackdata = Jquery_Ajax(savejson, rqurl, false);
            if (callbackdata.indexOf('成功')) {
                var stopId = '{StopId:"' + StopId + '"}';
                var data = JSON.parse(Jquery_Ajax(stopId, "/BusPoles/PolesNewList/StopIdChange", false));
                if (data.length > 0) {
                    var e = data[0];
                    var str_div = "<div style='font-size:12px;'><table><tr><td>站点站址</td><td>" + e.StationAddress + "</td></tr><tr><td>停靠线路</td><td>" + e.LineList + "</td></tr><tr><td>站点编号</td><td><a _data='stopid' href='javascript:;' onclick='javascript:openimage(\"" + e.StopId + "\")'>" + e.StopId + "</a></td></tr><tr><td>站点照片</td><td><a href='javascript:;' onclick='javascript:openimage(\"" + e.StopId + "\")'>点击查看照片</a></td></tr></table></div>";
                    currentEditPoint.info = new MInfoWindow("当前站点:" + e.StationName, str_div, "20px", "20px");
                    MEvent.addListener(currentEditPoint, "click", function (omarker) {
                        omarker.openInfoWindow(); omarker.openInfoWindow();
                    });
                }
            }
        });

    } catch (ex) {

    }
}


//检查指定的 MPoint 对象是否在搜索范围内。  
function check(pt, min, max) {
    return parseFloat(pt.lon) >= parseFloat(min.lon) &&
                   parseFloat(pt.lon) <= parseFloat(max.lon) &&
                   parseFloat(pt.lat) <= parseFloat(max.lat) &&
                   parseFloat(pt.lat) >= parseFloat(min.lat);
}
function removeAllOverlays() {
    maplet.clearOverlays(true);
    maplet.refresh();
}
$(function () {
   
    initMap();
    $('.btline').click(function () {

        removeAllOverlays();
        Jquery_SyncAjax({ "lineName": $("#RoadLine").val() }, "/BusMap/MapEdit/getRoadLineList", true, serchcallback);

    });


});
/**/
function serchcallback(data) {
     upPolyline = null;
     downPolyline = null;
     upStrLonLat = null;
     downStrLonLat = null;
    $('.jieguo1 .result001').remove();
    roadLonLats = new Map();
    if (data.Code == 1) {
        if (data.UpLine != null && data.UpLine != undefined) {
            var upLine = data.UpLine;
            var listLevel = upLine.ListLevel;
            roadLonLats.set(0, upLine.LonLats)
            var listRoad = "<div class='result001'>";
            listRoad += "<div class='selcheck01'> ";
            listRoad += "<div class='lis'>";
            listRoad += "</div>";
            listRoad += "<div class='sx1'>";
            listRoad += "上行 ";
            listRoad += "</div>";
            listRoad += "<div class='s12' onclick='s12(this)'>";
            listRoad += "<span>";
            listRoad += "显示轨迹";
            listRoad += "</span>";
            listRoad += "</div>";
            listRoad += "<div class='s15' onclick='s15(this)'>";
            listRoad += "<span>";
            listRoad += "显示站点 ";
            listRoad += "</span>  ";
            listRoad += "</div>";
            listRoad += "<div class='s14' onclick='s14(0,\"" + data.RoadLine + "\",\"" + upLine.ToDirection + "\")'>";
            listRoad += "保存";
            listRoad += "</div>";
            listRoad += "<div class='clear'>  ";
            listRoad += "</div>";
            listRoad += "</div>";
            listRoad += "<div class='sxcont'> ";
            listRoad += "<div class='restit1' onclick='toggleLevelList(this)'>";
            listRoad += "<div class='restitleft1'>";
            listRoad += upLine.StartPoint + " -> " + upLine.ToDirection;
            listRoad += "</div>";
            listRoad += "<div class='restitright1' id='reone'>";
            listRoad += "<a href='javascript:void(0);'>  ";
            listRoad += "<span class='zdicon01'> ";
            listRoad += "</span>  ";
            listRoad += "</a> ";
            listRoad += "</div>";
            listRoad += "<div class='clear'> ";
            listRoad += "</div>";
            listRoad += "</div>";
            listRoad += "<div class='sellist1' style='display:none' >";
            listRoad += "<table cellspacing='0' cellpadding='0' width='100%'><tbody>";
            levelMap.set(0, listLevel);
            listLevel.forEach(function (e) {
                listRoad += "<tr> ";
                listRoad += "<td width='10%'>";
                listRoad += e.LevelId;
                listRoad += "</td>";
                listRoad += "<td width='30%'>";
                listRoad += e.StopId;
                listRoad += "</td>";
                listRoad += "<td width='40%'>";
                listRoad += e.LevelName;
                listRoad += "</td>";
                listRoad += "<td width='20%'>";
                listRoad += "<img src='/Resource/Images/editic.png' onclick='editPoint(this,0)' style='cursor:pointer'/>";
                listRoad += "</td>";
                listRoad += "</tr>";
            });



            listRoad += "</tbody></table>";
            listRoad += "</div>";
            listRoad += "</div>";
            listRoad += "</div>";
            $(listRoad).appendTo($('.jieguo1'));
           // upStrLonLat = upLine.LonLats
        }


        if (data.DownLine != null && data.DownLine != undefined) {
           
            var downLine = data.DownLine;
            var listLevel = downLine.ListLevel;
           // downStrLonLat = downLine.LonLats;
            roadLonLats.set(1, downLine.LonLats)
            var listRoad = "<div class='result001'>";
            listRoad += "<div class='selcheck01'> ";
            listRoad += "<div class='lis'>";
            listRoad += "</div>";
            listRoad += "<div class='sx1'>";
            listRoad += "下行 ";
            listRoad += "</div>";
            listRoad += "<div class='s12' onclick='s12(this)'>";
            listRoad += "<span>";
            listRoad += "显示轨迹";
            listRoad += "</span>";
            listRoad += "</div>";
            listRoad += "<div class='s15' onclick='s15(this)'>";
            listRoad += "<span>";
            listRoad += "显示站点 ";
            listRoad += "</span>  ";
            listRoad += "</div>";
            listRoad += "<div class='s14' onclick='s14(1,\"" + data.RoadLine + "\",\"" + downLine.ToDirection + "\")'>";
            listRoad += "保存";
            listRoad += "</div>";
            listRoad += "<div class='clear'>  ";
            listRoad += "</div>";
            listRoad += "</div>";
            listRoad += "<div class='sxcont'> ";
            listRoad += "<div class='restit1' onclick='toggleLevelList(this)'>";
            listRoad += "<div class='restitleft1'>";
            listRoad += downLine.StartPoint + " -> " + downLine.ToDirection;
            listRoad += "</div>";
            listRoad += "<div class='restitright1' id='reone'>";
            listRoad += "<a href='javascript:void(0);'>  ";
            listRoad += "<span class='zdicon01'> ";
            listRoad += "</span>  ";
            listRoad += "</a> ";
            listRoad += "</div>";
            listRoad += "<div class='clear'> ";
            listRoad += "</div>";
            listRoad += "</div>";
            listRoad += "<div class='sellist1' style='display:none' >";
            listRoad += "<table cellspacing='0' cellpadding='0' width='100%'><tbody>";
            levelMap.set(1, listLevel);
            listLevel.forEach(function (e) {
                listRoad += "<tr> ";
                listRoad += "<td width='10%'>";
                listRoad += e.LevelId;
                listRoad += "</td>";
                listRoad += "<td width='30%'>";
                listRoad += e.StopId;
                listRoad += "</td>";
                listRoad += "<td width='40%'>";
                listRoad += e.LevelName;
                listRoad += "</td>";
                listRoad += "<td width='20%'>";
                listRoad += "<img src='/Resource/Images/editic.png' onclick='editPoint(this,1)' style='cursor:pointer'/>";
                listRoad += "</td>";
                listRoad += "</tr>";
            });



            listRoad += "</tbody></table>";
            listRoad += "</div>";
            listRoad += "</div>";
            listRoad += "</div>";
            $(listRoad).appendTo($('.jieguo1'));
        }
    } else {

        layer.alert(data.Msg);
    }

}

function editPoint(id, i) {

    var index = $(id).parent().parent().parent().find('img').index(id);
    var temp = levelPointMap.get(i);
    if (temp != undefined) {
        var thisPoint = temp.get(index);
        if (thisPoint != undefined) {
            var poi = thisPoint.pt;
            maplet.centerAndZoom(new MPoint(poi.lon,poi.lat), 15);
            thisPoint.openInfoWindow();
            thisPoint.openInfoWindow();
        } else {
            var marker = showOnlyPoint(i, index);
            if (marker != undefined) {
                marker.openInfoWindow();
                marker.openInfoWindow();
            }
        }
    }
    if (temp == undefined) {
        var marker = showOnlyPoint(i, index);
        if (marker != undefined) {
            marker.openInfoWindow();
            marker.openInfoWindow();
        }
    }
}


function s12(id) {
    var index = $('.result001 .s12').index($(id));
    if ($(id).hasClass("selected")) {
        $(id).removeClass("selected");
        $(id).find('span').text("显示轨迹");
        var lineP = roadPolyline.get(index);
        if (lineP != undefined) {
            maplet.removeOverlay(lineP);
        }
    }
    else {
        $(id).addClass("selected");
        $(id).find('span').text("隐藏轨迹");

        var lonLats = roadLonLats.get(index);
        var lineP = drawPolyline(lonLats, index)
        if (lineP != undefined) {
            roadPolyline.set(index, lineP);
        }
    }
}

function s14(ii, road, todir) {
    if (ii == 0) {

        if (upStrLonLat != null && upStrLonLat != null) {
            var stopId = '{roadline:"' + road + '",linelatlon:"' + upStrLonLat + '",toDirection:"' + todir + '"}';
            var data = JSON.parse(Jquery_Ajax(stopId, "/BusMap/MapEdit/UpdateRoadTrace", false));
            if (data.IsSuccess) {
                layer.alert("轨迹保存成功！", { icon: 1 });
            } else {
                layer.alert(data.Message, { icon: 2 });
            }
            upStrLonLat = null;
        }




        UpLevelMap.removeAll();

    } else {
        if (downStrLonLat != null && downStrLonLat != null) {
            var stopId = '{roadline:"' + road + '",linelatlon:"' + downStrLonLat + '",toDirection:"' + todir + '"}';
            var data = JSON.parse(Jquery_Ajax(stopId, "/BusMap/MapEdit/UpdateRoadTrace", false));
            if (data.IsSuccess) {
                layer.alert("轨迹保存成功！", { icon: 1 });
            } else {
                layer.alert(data.Message, { icon: 2 });
            }
            downStrLonLat = null;
        }
        DownLevelMap.removeAll();
    }
}

function s15(id) {
    var index = $('.result001 .s15').index($(id));
    if ($(id).hasClass("selected")) {
        $(id).removeClass("selected");
        $(id).find('span').text("显示站点");
        var levelP = levelPointMap.get(index);
        if (levelP != undefined) {
            levelP.keys.forEach(function (e) {
                maplet.removeOverlay(levelP.get(e));
            });
            levelPointMap.remove(index);
        }
    }
    else {
        $(id).addClass("selected");
        $(id).find('span').text("隐藏站点");
        var levelPoint = levelMap.get(index);
        showPoint(levelPoint, index);

    }
}
/*id 上下行标记：0：上行，1:下行
i:第几个站级

*/

var UpLevelMap = new Map();
var DownLevelMap = new Map();
function showOnlyPoint(id, ii) {

    var leveltemps = levelMap.get(id);
    if (leveltemps.length < ii) {
        BookMarkPoint("Station");

    } else {
        var e = leveltemps[ii];
        var labl = e.LevelName;
        var lat = e.LonLat.split(',')[0]; //经度
        var lon = e.LonLat.split(',')[1]; //纬度
        var point = new MPoint(lat, lon); //坐标
        marker = new MMarker(point, new MIcon("/Resource/Images/icons/1/icon_03.png", 16, 16));
        marker.setLabel(new MLabel(labl), true); //添加标签
        marker.label.setVisible(false)//隐藏标签

        var lineList = e.LineList.split("、");
        var strlineList = "";
        for (var i = 0; i < lineList.length; i++) {
            if (strlineList != "") {
                strlineList += "、" + lineList[i];

            } else {
                strlineList += lineList[i];
            }
        }
        var str_div = "<div style='font-size:12px;'><table><tr><td>站点站址</td><td>" + e.StationAddress + "</td></tr><tr><td>停靠线路</td><td>" + strlineList + "</td></tr><tr><td>站点编号</td><td><a href='javascript:;' onclick='javascript:openimage(\"" + e.StopId + "\")' _data='stopid'>" + e.StopId + "</a></td></tr><tr><td>站点照片</td><td><a href='javascript:;' onclick='javascript:openimage(\"" + e.StopId + "\")' >点击查看照片</a></td></tr></table></div>";
        marker.info = new MInfoWindow("当前站点:" + labl, str_div, "20px", "20px");

        MEvent.addListener(marker, "click", function (omarker) {
            //omarker.pt.getPid();
            omarker.pt.bEditable = true;
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
            maplet.setMode("pan");
            mk.bEditable = false;
            
            var stopId = $(mk.info.content).find("a[_data='stopid']").text();
            if (id == 0) {
                UpLevelMap.set(stopId, mk.pt.getPid());
            } else {
                DownLevelMap.set(stopId, mk.pt.getPid());
            }
        });
        var pointList = new Map();
        var tempx = levelPointMap.get(id);
        marker.setContextMenu(menu);
        if (tempx != undefined) {
            pointList = tempx;
            pointList.set(ii, marker);
            levelPointMap.set(id, pointList);
        } else {
            pointList.set(ii, marker);
            levelPointMap.set(id, pointList);
        }
        maplet.centerAndZoom(point, 15);
        maplet.addOverlay(marker);
        return marker;
    }
}


function showPoint(level, id) {
    var pointList = new Map();
    var j = 0;
    var tempx = levelPointMap.get(id);
    if (tempx != undefined) {
        tempx.keys.forEach(function (e) {
            maplet.removeOverlay(tempx.get(e));
        });
    }

    level.forEach(function (e) {
        if (e.LonLat != "") {
            var labl = e.LevelName;
            var lat = e.LonLat.split(',')[0]; //经度
            var lon = e.LonLat.split(',')[1]; //纬度
            var point = new MPoint(lat, lon); //坐标
            marker = new MMarker(point, new MIcon("/Resource/Images/icons/1/icon_03.png", 16, 16));
            marker.setLabel(new MLabel(labl), true); //添加标签
            marker.label.setVisible(false)//隐藏标签

            var lineList = e.LineList.split("、");
            var strlineList = "";
            for (var i = 0; i < lineList.length; i++) {
                if (strlineList != "") {
                    strlineList += "、" + lineList[i];

                } else {
                    strlineList += lineList[i];
                }
            }
            var str_div = "<div style='font-size:12px;'><table><tr><td>站点站址</td><td>" + e.StationAddress + "</td></tr><tr><td>停靠线路</td><td>" + strlineList + "</td></tr><tr><td>站点编号</td><td><a  _data='stopid' href='javascript:;' onclick='javascript:openimage(\"" + e.StopId + "\")'>" + e.StopId + "</a></td></tr><tr><td>站点照片</td><td><a href='javascript:;' onclick='javascript:openimage(\"" + e.StopId + "\")'>点击查看照片</a></td></tr></table></div>";
            marker.info = new MInfoWindow("当前站点:" + labl, str_div, "20px", "20px");

            MEvent.addListener(marker, "click", function (omarker) {
                //omarker.pt.getPid();
                omarker.pt.bEditable = true;
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
                maplet.setMode("pan");
                
                mk.bEditable = false;
                var stopId = $(mk.info.content).find("a[_data='stopid']").text();
                if (id == 0) {
                    UpLevelMap.set(stopId, mk.pt.getPid());
                } else {
                    DownLevelMap.set(stopId, mk.pt.getPid());
                }
            });
            pointList.set(j, marker);
            marker.setContextMenu(menu);
            maplet.addOverlay(marker);
        }
        j++;
    });
    levelPointMap.set(id, pointList);
}


function toggleLevelList(id) {
    $(id).next().toggle();
    if (!$(id).next().is(':visible')) {
        $(id).find('.restitright1').css({ "background-position-y": "-32px" });
    } else {
        $(id).find('.restitright1').css({ "background-position-y": "0px" });
    }
}

function drawPolyline(polyline, ii) {
    var points_temp = new Array();
    if (polyline != null && polyline != undefined) {
        var maxLon = -180;
        var maxLat = -180;
        var minLon = 180;
        var minLat = 180;


        var LineLatLon = polyline.split(';');
        for (var i = 0; i < LineLatLon.length; i++) {
            if (LineLatLon[i] != undefined && LineLatLon[i] != null && LineLatLon[i] != "") {
                var lat = parseFloat(LineLatLon[i].split(',')[0]);
                var lon = parseFloat(LineLatLon[i].split(',')[1]);

                if (lat < minLat) {
                    minLat = lat;
                } if (lat > maxLat) {
                    maxLat = lat;
                }
                if (lon < minLon) {
                    minLon = lon;
                } if (lon > maxLon) {
                    maxLon = lon;
                }
                var point = new MPoint(lat, lon);
                points_temp.push(point);
            }
        }

        //创建笔刷将所有的点连接起来
        var brush = new MBrush();
        brush.arrow = false;
        brush.stroke = 4;
        brush.fill = false;
        if (ii == 0)
            brush.color = '#416db4';
        else
            brush.color = '#149714';
        brush.bgcolor = 'red';
        brush.overlap = {
            // Boolean类型，是否显示边框突出。  
            enable: true,
            // Int类型，突出边框宽度。  
            stroke: 1,
            // String类型,突出边框颜色值，可以是十六进制数字 HTML 样式或颜色名称，例如，使用 #ff0000 和 red 都可以。  
            color: "#ffffff",
            // Int类型,突出边框透明度，取值范围0-100。  
            transparency: 80
        }
        var mappolyline = new MPolyline(points_temp, brush);
        if (ii == 0) {
            upPolyline = mappolyline;
        } else {
            downPolyline = mappolyline;
        }
        mappolyline.setEditable(true);
       
        maplet.addOverlay(mappolyline);
        MEvent.addListener(maplet, "edit", EditLine);
        if (maxLon != -180 & maxLat != -180 && minLon != 180 & minLat != 180) {
            maplet.centerAndZoom(new MPoint((maxLat + minLat) / 2, (maxLon + minLon) / 2), 13);
        }
        return mappolyline;
    }
}

var upPolyline = null;
var downPolyline = null;
var upStrLonLat = null;
var downStrLonLat = null;
//编辑线段
function EditLine(overlay) {
    var strLonLat = "";
    for (var i = 0; i < overlay.pts.length; i++) {
        strLonLat += overlay.pts[i].getPid() + ";";
    }
    if (overlay == upPolyline) {
        upStrLonLat = strLonLat;
        console.log(strLonLat);
    }
    else {
        downStrLonLat = strLonLat;
    }
}

//添加地图点
function BookMarkPoint(p_type) {
    if (maplet.getZoomLevel() < 13) {
        maplet.setZoomLevel(13);
    }
    if (p_type == "drawline") {
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
    else if (p_type == "measure") {
        maplet.setMode("measure");
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
    var strdiv = "<table style='margin-left: 12px; border-style: solid; border-width: 0px; border-color: #CCCCCC #999 #999 #CCCCCC;background-color: White;'><tbody><tr><td>线路：<select id='Area' name='Area' onchange='selectitem(this)' style=' display: block; width: 120px;'><option value=''>-请选择-</option></select></td></tr><tr><td></td><td style='float: right;'><input type='button' id='BtnSave' value='保存' onclick='SaveStopPoint(this)' style='padding-top: 2px;height: 25px'></td></tr></tbody></table>";

    polyline.info = new MInfoWindow("信息窗口标题", strdiv);


    maplet.addOverlay(polyline);
    polyline.openInfoWindow();
    polyline.openInfoWindow(); 
    polyline.setEditable(true);
}

var currentEditPoint = null;
/*添加标注点*/
function PointAdd(strUrl) {
    if (strUrl.action == "add") {
        avBubble.width = 280;
        avBubble.height = 240;

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
            var strdiv = "<div style='display:none' _title='lonlat'>" + strUrl.point.lon + "," + strUrl.point.lat + "</div><table style='margin-left: 12px; border-style: solid; border-width: 0px; border-color: #CCCCCC #999 #999 #CCCCCC;background-color: White;'><tbody><tr><td>环域：<select id='Area' name='Area' onchange='selectitem(this)' style=' display: block; width: 120px;'><option value=''>-请选择-</option><option value='内环内'>内环内</option><option value='外环内'>外环内</option><option value='外环外'>外环外</option><option value='中环内'>中环内</option></select></td><td>区属<select id='District' style='display: block; width: 120px;' runat='server' onchange='selectitem(this)'><option label='' value=''></option></select></td></tr><tr><td>路名<select id='RoadName' runat='server' style='display: block; width: 120px;' onchange='selectitem(this)'><option label='' value=''></option></select></td><td>站名<select id='StationName' style='display: block; width: 120px;' onchange='selectitem(this)'><option label='' value=''></option></select></td></tr><tr><td>车向<select id='PathDirection' style='display: block; width: 120px' onchange='selectitem(this)'><option label='' value=''></option></select></td><td>站址<select id='StationAddress' style='display: block; width: 120px;' onchange='selectitem(this)'><option label='' value=''></option></select></td></tr><tr><td>站点编号<input id='txtStopId' name='txtStopId' type='text' style='display: block; width: 120px;' onblur='StopIdBlur(this)'></td><td><input id='txtStrLatlon' type='hidden'></td></tr><tr><td></td><td style='float: right;'><input type='button' id='BtnSave' value='保存' onclick='SaveStopPoint(this)' style='padding-top: 2px;height: 25px'></td></tr></tbody></table>";
            marker.info = new MInfoWindow("关联信息", strdiv);
        }

        MEvent.addListener(marker, "drag", function (mk) {
            maplet.setMode("pan");
            mk.bEditable = false;
            $(mk.info.content).find("div[_title='lonlat']").text(mk.pt.getPid());
            mk.openInfoWindow();
            mk.openInfoWindow();


        });
        MEvent.addListener(marker, "click", function (mk) {
            currentEditPoint = mk;
        });
        currentEditPoint = marker;
        marker.setShadow(new_shado, true)
        maplet.addOverlay(marker);
        marker.openInfoWindow();
        marker.openInfoWindow();
        marker.setContextMenu(menu);
        $("#txtStrLatlon").val(strUrl.point.pid);
        maplet.setMode("pan");
        return true;
    }
}


function clearMap() {
    maplet.customInfoWindow = true;
    maplet.clearOverlays(true);
    maplet.refresh();

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

/*键值对*/
function Map() {
    this.keys = new Array();
    this.data = new Array();
    //添加键值对
    this.set = function (key, value) {
        if (this.data[key] == null) { //如键不存在则身【键】数组添加键名
            this.keys.push(key);
        }
        this.data[key] = value; //给键赋值
    };
    //获取键对应的值
    this.get = function (key) {
        return this.data[key];
    };
    //去除键值，(去除键数据中的键名及对应的值)
    this.remove = function (key) {
        this.keys.pop(key);
        this.data[key] = null;
    };
    //清除所有
    this.removeAll = function () {
        var keyArr = this.keys;
        while (keyArr.length > 0) {
            var k = keyArr[0];
            this.keys.pop(k);
            this.data[k] = null;
        }
    }
    //判断键值元素是否为空
    this.isEmpty = function () {
        return this.keys.length == 0;
    };
    //获取键值元素大小
    this.size = function () {
        return this.keys.length;
    };
}