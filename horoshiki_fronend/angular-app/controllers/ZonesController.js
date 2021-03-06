/**
 * Created by onyushkindv on 10.10.2016.
 */


var zonesControllers = angular.module('zonesControllers', []);

zonesControllers.controller('ZonesViewController', ['$scope', '$rootScope', '$location', 'BackendService', 'SubdivisionService', 'PlatformsService', 'ZonesService', 'DictionaryService','$rootScope',
    function ($scope, $rootScope, $location, BackendService, SubdivisionService, PlatformsService, ZonesService, DictionaryService, $rootScope) {

        var obj_index = 0;

        $scope.model = {};
        $scope.model.objects = [];
        $scope.model.zoneForm = {};
        $scope.model.zoneForm.active = false;
        $scope.model.zoneForm.coordinates = [];
        $scope.model.subdivisionCenter = {};

        $scope.model.def = {};
        $scope.model.def.options = {};

        $scope.map = {};

        $scope.initMap = function (target) {
            $scope.map = target;
        }

        $scope.setMapCenter = function (center) {
            if (center != undefined && center != "")
                $scope.map.setCenter(JSON.parse(center));
        }

        $scope.addedPolygon = function (terget) {
            // console.log(terget.properties.get('index'));
            for ($index in $scope.model.objects) {
                if ($scope.model.objects[$index].index == terget.properties.get('index')) {
                    $scope.changeActiveZone($scope.model.objects[$index].index);
                }
            }

            $scope.map.geoObjects.each(function (geoObject) {
                if (geoObject.properties.get('index') == terget.properties.get('index') && geoObject.properties.get('isNew') == true) {
                    geoObject.editor.startDrawing();
                }
            });
        }

        $scope.addObject = function () {

            if($scope.subdivisionIsDisabled)
                return false;

            var zone = {
                index: obj_index,
                name: 'Зона ' + obj_index,
                id: 0,
                type: $scope.model.deliveryZoneTypes[0],
                platformId: JSON.parse($scope.model.platform).Id,
                zindex: JSON.parse($scope.model.zoneForm.zoneType).ZIndex,
                options: {
                    editorDrawingCursor: 'crosshair',
                    editorMaxPoints: 200,
                    fillColor: JSON.parse($scope.model.zoneForm.zoneType).Background,
                    strokeColor: JSON.parse($scope.model.zoneForm.zoneType).BorderColor,
                    zIndex: JSON.parse($scope.model.zoneForm.zoneType).ZIndex,
                    zIndexActive: JSON.parse($scope.model.zoneForm.zoneType).ZIndex,
                    // fillColor: '#ccc',
                    // strokeColor: '#ccg',
                    fillOpacity: '0.2',
                    strokeWidth: 1
                },
                polygon: {
                    geometry: {
                        type: 'Polygon',
                        coordinates: []
                    },
                    properties: {index: obj_index, isNew: true}
                }
            }
            $scope.model.objects.push(zone);
            obj_index++;
        }

        $scope.deleteObject = function () {
            if($scope.subdivisionIsDisabled)
                return false;

            if ($scope.model.zoneForm.active && $scope.model.zoneForm.index != undefined && $scope.model.zoneForm != '') {

                for ($index in $scope.model.objects) {
                    if ($scope.model.objects[$index].index == $scope.model.zoneForm.index) {
                        // console.log($scope.model.objects[$index].id);
                        ZonesService.delete($scope.model.objects[$index].id).success(function (result) {
                            if (result.Success == 1) {

                                $scope.model.objects = $scope.model.objects.filter(function (item) {
                                    return item !== $scope.model.objects[$index];
                                });

                                $scope.map.geoObjects.each(function (geoObject) {
                                    if (geoObject.properties.get('index') == $scope.model.zoneForm.index) {
                                        $scope.map.geoObjects.remove(geoObject);
                                    }
                                });

                            } else {
                                displayErrorMessage(result.ReasonMessage);
                            }
                        }).error(function (result, status) {
                            httpErrors($location.url(), status);
                        });

                        break;
                    }
                }

                $scope.model.zoneForm.active = false;
            }
        }

        $scope.changeActiveZoneWithMap = function (event) {
            $scope.changeActiveZone(event.get('target').properties.get('index'));
        }

        $scope.changeActiveZoneWithList = function (index) {
            for ($index in $scope.model.objects) {
                if ($scope.model.objects[$index].index == index) {
                    for ($indexP in $scope.model.platforms) {
                        if ($scope.model.platforms[$indexP].Id == $scope.model.objects[$index].platformId) {
                            $scope.setMapCenter($scope.model.platforms[$indexP].YandexMap);
                            break;
                        }
                    }
                    break;
                }
            }
            $scope.changeActiveZone(index);
        }


        $scope.changeActiveZone = function (index) {
            for ($index in $scope.model.objects) {
                if ($scope.model.objects[$index].index == index) {
                    $scope.model.zoneForm.name = $scope.model.objects[$index].name;
                    $scope.model.zoneForm.index = $scope.model.objects[$index].index;
                    $scope.model.zoneForm.active = true;

                    $scope.model.zoneForm.zoneType = angular.toJson($scope.model.objects[$index].type);


                    for ($indexP in $scope.model.platforms) {
                        if ($scope.model.platforms[$indexP].Id == $scope.model.objects[$index].platformId) {
                            $scope.model.platform = angular.toJson($scope.model.platforms[$indexP]);
                            $scope.model.zoneForm.platformId = JSON.parse($scope.model.platform).Id;
                            break;
                        }
                    }
                }
            }

            $scope.map.geoObjects.each(function (geoObject) {
                geoObject.options.set({fillOpacity: '0.2'});
                geoObject.editor.stopEditing();
                if (geoObject.properties.get('index') == index) {
                    geoObject.options.set({fillOpacity: '0.3'});
                    $scope.model.zoneForm.coordinates = geoObject.geometry.getCoordinates();
                    geoObject.editor.startEditing();
                }
                // console.log(JSON.stringify(geoObject.geometry.getCoordinates()));
            });

        }

        $scope.changeValueActiveZone = function () {
            for ($index in $scope.model.objects) {
                if ($scope.model.objects[$index].index == $scope.model.zoneForm.index) {
                    $scope.model.objects[$index].name = $scope.model.zoneForm.name;
                    $scope.saveZone();
                    break;
                }
            }
        }

        $scope.saveCenterPlatform = function () {

            if($scope.subdivisionIsDisabled)
                return false;

            var platform = JSON.parse($scope.model.platform);
            platform.YandexMap = JSON.stringify($scope.map.getCenter());

            PlatformsService.editCenter(platform.Id, platform.YandexMap).success(function (result) {
                if (result.Success == 1) {

                } else {
                    displayErrorMessage(result.ReasonMessage);
                }
            }).error(function (result, status) {
                httpErrors($location.url(), status);
            });

            for ($index in $scope.model.platforms) {
                if ($scope.model.platforms[$index].Id == platform.Id) {
                    $scope.model.platforms[$index] = platform;
                }
            }

            $scope.model.platform = JSON.stringify(platform);
            // console.log($scope.model.platform);
        };

        $scope.notActiveZone = function (event) {
            $scope.model.zoneForm.active = false;
            $scope.model.zoneForm.coordinates = [];
            $scope.map.geoObjects.each(function (geoObject) {
                geoObject.options.set({fillOpacity: '0.1'});
                geoObject.editor.stopEditing();
            });
        };

        $scope.changeGeometry = function (event) {
            // console.log("change geometry");
            var objects = $scope.model.objects;
            for ($index in objects) {
                if (event.get('target').properties.get('index') == objects[$index].index) {
                    // $scope.model.objects[$index].polygon.geometry.coordinates = event.get('target').geometry.getCoordinates();
                    $scope.model.zoneForm.coordinates = event.get('target').geometry.getCoordinates();
                    break;
                }
            }

            $scope.saveZone();
        };

        $scope.saveZone = function () {
            var objects = $scope.model.objects;
            for ($index in objects) {
                if ($scope.model.zoneForm.index == objects[$index].index) {
                    var zoneId = JSON.parse($scope.model.zoneForm.zoneType).Id;

                    var deliveryZone = {
                        Id: objects[$index].id,
                        Platform: {
                            "Id": JSON.parse($scope.model.platform).Id
                        },
                        DeliveryZoneType: {
                            "Id": zoneId
                        },
                        Name: objects[$index].name,
                        Сoordinates: JSON.stringify($scope.model.zoneForm.coordinates)
                    };

                    if (objects[$index].id == 0) {
                        ZonesService.add(deliveryZone).success(function (result) {
                            if (result.Success == 1) {
                                console.log("add");
                                $scope.model.objects[$index].id = result.Data;
                            } else {
                                displayErrorMessage(result.ReasonMessage);
                            }
                        }).error(function (result, status) {
                            httpErrors($location.url(), status);
                        });
                    } else {
                        ZonesService.edit(deliveryZone).success(function (result) {
                            if (result.Success == 1) {
                                console.log("edit");
                            } else {
                                displayErrorMessage(result.ReasonMessage);
                            }
                        }).error(function (result, status) {
                            httpErrors($location.url(), status);
                        });
                    }
                }
            }
        };

        $scope.changeSubdivision = function () {
            if($rootScope.currentUser.Position.Guid==enumPositions.chiefOperatingOfficer) {
                $scope.getAllPlatformBySubdivision(JSON.parse($scope.model.subdivision).Id);
                $scope.updateMapCoordinates($scope.model.platforms);
            }
        };

        $scope.changePlatform = function () {
            if ($scope.model.platform != undefined && $scope.model.platform != '' && $rootScope.currentUser.Position.Guid==enumPositions.chiefOperatingOfficer) {
                $scope.setMapCenter(JSON.parse($scope.model.platform).YandexMap);
                $scope.model.zoneForm.platformId = JSON.parse($scope.model.platform).Id;
            }
        };

        $scope.changeZoneType = function () {
            for ($index in $scope.model.objects) {
                if ($scope.model.objects[$index].index == $scope.model.zoneForm.index) {
                    $scope.model.objects[$index].type = JSON.parse($scope.model.zoneForm.zoneType);
                }
            }

            $scope.updateColorZone();
            $scope.saveZone();

        };

        $scope.updateColorZone = function () {
            $scope.map.geoObjects.each(function (geoObject) {
                if (geoObject.properties.get('index') == $scope.model.zoneForm.index) {
                    geoObject.options.set({fillColor: JSON.parse($scope.model.zoneForm.zoneType).Background});
                    geoObject.options.set({strokeColor: JSON.parse($scope.model.zoneForm.zoneType).BorderColor});
                    geoObject.options.set({zIndex: JSON.parse($scope.model.zoneForm.zoneType).ZIndex});
                    geoObject.options.set({zIndexActive: JSON.parse($scope.model.zoneForm.zoneType).ZIndex});
                    // console.log("123");
                }
            });
        };

        $scope.updateMapCoordinates = function (platforms) {

            if ($scope.map.geoObjects != undefined)
                $scope.map.geoObjects.removeAll();
            $scope.model.objects = [];

            if (platforms.length == undefined || platforms.length == 0)
                return;

            for (var $indexP in $scope.model.platforms) {
                var platform = $scope.model.platforms[$indexP];

                if (platform == undefined || platform.DeliveryZones == undefined || platform.DeliveryZones.length == 0)
                    return;

                for ($indexZ in platform.DeliveryZones) {
                    var deliveryZone = platform.DeliveryZones[$indexZ];

                    var zone = {
                        index: obj_index,
                        name: deliveryZone.Name,
                        id: deliveryZone.Id,
                        type: deliveryZone.DeliveryZoneType,
                        platformId: platform.Id,
                        options: {
                            editorDrawingCursor: 'crosshair',
                            editorMaxPoints: 200,
                            fillColor: deliveryZone.DeliveryZoneType.Background,
                            strokeColor: deliveryZone.DeliveryZoneType.BorderColor,
                            zIndex: deliveryZone.DeliveryZoneType.ZIndex,
                            zIndexActive: deliveryZone.DeliveryZoneType.ZIndex,
                            fillOpacity: '0.1',
                            strokeWidth: 1
                        },
                        polygon: {
                            geometry: {
                                type: 'Polygon',
                                coordinates: JSON.parse(deliveryZone.Сoordinates)
                            },
                            properties: {index: obj_index}
                        }
                    };

                    $scope.model.objects.push(zone);
                    obj_index++;
                }
            }


        };

        $scope.model.subdivisionIsDisabled = true;
        $scope.model.platformIsDisabled = true;

        $scope.getAllSubdivisions = function () {
            SubdivisionService.getSubdivisionsWithoutPaginate('Id', true).success(function (result) {
                if (result.Success == 1) {
                    $scope.model.subdivisions = result.Data.Data;
                    if ($scope.model.subdivisions.length != 0) {


                        if($rootScope.currentUser.Position.Guid==enumPositions.chiefOperatingOfficer){
                            $scope.model.subdivision = JSON.stringify($scope.model.subdivisions[0]);
                            $scope.subdivisionIsDisabled = false;
                        }
                        if($rootScope.currentUser.Position.Guid==enumPositions.manager){
                            console.log($rootScope.currentUser);

                            for($index in $scope.model.subdivisions){
                                if($scope.model.subdivisions[$index].Id == $rootScope.currentUser.Platform.SubDivision.Id){
                                    $scope.model.subdivision = JSON.stringify($scope.model.subdivisions[$index]);
                                    $scope.subdivisionIsDisabled = true;
                                }
                            }
                        }

                        $scope.getAllPlatformBySubdivision(JSON.parse($scope.model.subdivision).Id);
                    }
                } else {
                    displayErrorMessage(result.ReasonMessage);
                }
            }).error(function (result, status) {
                httpErrors($location.url(), status);
            });
        };

        $scope.getAllPlatformBySubdivision = function (id) {
            PlatformsService.getAllBySubdivision(id).success(function (result) {
                if (result.Success == 1) {
                    $scope.model.platforms = result.Data;
                    if ($scope.model.platforms.length != 0) {

                        if($rootScope.currentUser.Position.Guid==enumPositions.chiefOperatingOfficer) {
                            $scope.model.platform = JSON.stringify($scope.model.platforms[0]);
                            $scope.model.zoneForm.platformId = $scope.model.platforms[0].Id;
                            $scope.platformIsDisabled = false;
                        }
                        if($rootScope.currentUser.Position.Guid==enumPositions.manager){
                            for($index in $scope.model.platforms){
                                if($scope.model.platforms[$index].Id == $rootScope.currentUser.Platform.Id){
                                    $scope.model.platform = angular.toJson($scope.model.platforms[$index]);
                                    $scope.model.zoneForm.platformId = $scope.model.platforms[$index].Id;
                                    $scope.platformIsDisabled = true;
                                }
                            }
                        }
                    }
                    $scope.updateMapCoordinates($scope.model.platforms);

                    $scope.model.subdivisionCenter = JSON.parse($scope.model.platform).YandexMap;
                    // console.log($scope.model.subdivisionCenter);
                } else {
                    displayErrorMessage(result.ReasonMessage);
                }
            }).error(function (result, status) {
                httpErrors($location.url(), status);
            });
        };

        $scope.getDeliveryZoneTypes = function () {
            DictionaryService.getDeliveryZoneTypes().success(function (result) {
                if (result.Success == 1) {
                    $scope.model.deliveryZoneTypes = result.Data;
                    $scope.model.zoneForm.zoneType = angular.toJson($scope.model.deliveryZoneTypes[0]);
                } else {
                    displayErrorMessage(result.ReasonMessage);
                }
            }).error(function (result, status) {
                httpErrors($location.url(), status);
            });


        };

        // -----------------------------------------------------------------------------------

        $scope.model.geoAddress = {};
        $scope.model.geoAddress.isCheck = 'false';
        $scope.model.geoAddress.platformName = '';
        $scope.model.geoAddress.zoneTypeName = '';

        $scope.initGeocoder = function () {

            $("#autocomplete").autocomplete({
                source: function (request, response) {
                    ymaps.suggest(request.term).then(function (items) {
                        var s = [];
                        for ($index in items) {
                            s[$index] = items[$index].value;
                        }
                        response(s);
                    });
                },
                select: function (request, response) {
                    // $scope.$apply(function () {
                        $("#autocomplete").val(response.item.value);
                        // $scope.model.geoAddress = response.item.value;
                    // });

                }
            });

        }

        // $scope.getAddr = function (cords) {
        //     $scope.geoAddress = cords;
        // }

        $scope.searchAddress = function () {
            $scope.model.geoAddress.isCheck = 'load';
            var myGeocoder = ymaps.geocode($("#autocomplete").val());

            myGeocoder.then(
                function (res) {
                    var arr = [];
                    var geoAddress = {};

                    for (var i = 0; i < $scope.map.geoObjects.getLength(); i++) {

                        if ($scope.map.geoObjects.get(i).geometry.getCoordinates()[0].length != 0 && $scope.map.geoObjects.get(i).geometry.contains(res.geoObjects.get(0).geometry.getCoordinates())) {
                            $scope.map.geoObjects.get(i).properties.get("index")
                            // console.log($scope.map.geoObjects.get(i).properties.get("index") + " -- contains");

                            var objects = $scope.model.objects;
                            for ($index in objects) {
                                if ($scope.map.geoObjects.get(i).properties.get("index") == objects[$index].index) {
                                    arr.push(objects[$index]);
                                }
                            }
                        }
                    }

                    // console.log(arr[arr.length - 1].type.Value);

                    console.log(arr.length);

                    if (arr.length != 0) {
                        arr.sort(compareZone);


                        $scope.model.geoAddress.isCheck = 'true';
                        $scope.model.geoAddress.zoneTypeName = arr[arr.length - 1].type.Value;

                        for(var $index in $scope.model.platforms){
                            console.log($scope.model.platforms[$index]);
                            if($scope.model.platforms[$index].Id == arr[arr.length-1].platformId){
                                $scope.model.geoAddress.platformName = $scope.model.platforms[$index].Name;
                            }
                        }


                        console.log($scope.model.geoAddress);

                    }
                    else{
                        $scope.model.geoAddress.isCheck = 'false';
                        $scope.model.geoAddress.zoneTypeName = '';
                        $scope.model.geoAddress.platformName = '';
                    }


                    $rootScope.$digest();
                },
                function (err) {
                    // обработка ошибки
                }
            );




        }

        // $scope.checkAddressinZoneClose = function () {
        //     $scope.model.geoAddress.isCheck = false;
        //     $scope.model.geoAddress.zoneTypeName = '';
        //     $scope.model.geoAddress.platformName = '';
        // }

        function compareZone(a, b) {
            if (a.type.ZIndex < b.type.ZIndex)
                return -1;
            if (a.type.ZIndex > b.type.ZIndex)
                return 1;
            return 0;
        }

        $scope.initGeocoder();

        // -----------------------------------------------------------------------------------

        $rootScope.$watch('currentUserLoaded', function(){
            if($rootScope.currentUserLoaded == true){
                $scope.getAllSubdivisions();
                $scope.getDeliveryZoneTypes();
            }
        });

    }
]);
