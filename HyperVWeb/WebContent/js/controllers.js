'use strict';

function UpdateVirtualMachines($scope, $http) {
    if (!lastUpdateSuccessed) {
        return;
    }
    var url = "/api/vm/";

    var updateVM = function (data) {
        $scope.VM = data;
        lastUpdateSuccessed = true;
    };
    lastUpdateSuccessed = false;
    $http.get(url)
        .success(updateVM);
}

function UpdateVirtualSwitches($scope, $http) {
    var url = "/api/switch/";

    var updateSW = function (data) {
        $scope.SW = data;
    };
    $http.get(url)
        .success(updateSW);
}

/* Controllers */
function VMListCtrl($scope, $http, $location) {
    // Wrapper method for changing the location
    $scope.setLocation = function (url) {
        $location.url(url);
    };
    UpdateVirtualMachines($scope, $http);
}

var updateEnabled = false;
var lastUpdateSuccessed = true;
function MainCtrl($scope, $http, $location, $route) {
    if (!updateEnabled)
    {
        updateEnabled = true;
        setInterval(function () {
            $scope.$apply(function () {
                UpdateVirtualMachines($scope, $http);
            });
        }, 1000);
    }

    UpdateVirtualMachines($scope, $http);

    $scope.mySelections = [];
    $scope.gridOptions = { data:'VM',
        footerVisible:false,
        displaySelectionCheckbox:false,
        multiSelect:false,
        selectedItems:$scope.mySelections,
//        jqueryUITheme: true,
        columnDefs:[
            //{ field:"ElementName", displayName:"Name"},
            //{ field: "EnabledState", displayName: "State"},
            //{ field: "OnTimeInMilliseconds", displayName: "Uptime", cellTemplate: "<div class='ng-scope ngCellText colt1'><span>{{row.entity[col.field] | duration:'hh:mm:ss:sss'}}</span></div>" }
            { field: "VMName", displayName: "Name" },
            { field: "State", displayName: "State" },
            { field: "MemoryAssigned", displayName: "MemoryAssigned", cellTemplate: "<div class='ng-scope ngCellText colt1'><span>{{row.entity[col.field] /(1024*1024) + ' Mb'}}</span></div>" },
            { field: "Uptime", displayName: "Uptime" }
        ]};
    var updateGrid = function () {
        var tmp = $scope.mySelections[0];
        var selected = false;
        if (!!tmp) {
            var id = tmp.Id;
            if (!!id) {
                angular.forEach($scope.VM, function (data, index) {
                        if (data.Id == id) {
                            $scope.gridOptions.selectItem(index, true);
                            selected = true;
                        }
                    }
                );
            }
        }
        if (!selected) {
            $scope.gridOptions.selectItem(0, true);
        }
    };
    $scope.$on('ngGridEventData', updateGrid);


    var actionUrls = {};
    actionUrls["Start"] = "/api/start/";
    actionUrls["Pause"] = "/api/pause/";
    actionUrls["Save"] = "/api/save/";
    actionUrls["Stop"] = "/api/stop/";
    actionUrls["Shutdown"] = "/api/shutdown/";


    var PerformAction = function (action) {
        var url = actionUrls[action];
        var data = {};
        data.Id = $scope.mySelections[0].Id;
        data.HyperVHost = "localhost";
        $http.post(url, data).success(function () {
            UpdateVirtualMachines($scope, $http);
        });
    }
    $scope.PerformAction = PerformAction;

    $scope.$activeTab = $route.current.activetab;
}

MainCtrl.$inject = ["$scope", "$http", "$location", "$route"];

function DetailsCtrl($scope, $http, $location) {
    // Wrapper method for changing the location
    $scope.setLocation = function (url) {
        $location.url(url);
    };

    var url = "/api/VMDetails/";
    $http.get(url)
        .success(function (data) {
            var obj = angular.fromJson(data);
            var obj2 = angular.fromJson(obj);
            $scope.data = obj2;
        });
}

function NewVmCtrl($scope, $http, $location) {
    // Wrapper method for changing the location
    $scope.setLocation = function (url) {
        $location.url(url);
    };
}

function HelpCtrl($scope, $http, $location, $route) {
      $scope.$activeTab = $route.current.activetab;

    // Wrapper method for changing the location
//    $scope.setLocation = function (url) {
//        $location.url(url);
//    };
}

function SwitchCtrl($scope, $http, $location) {
    // Wrapper method for changing the location
    $scope.setLocation = function (url) {
        $location.url(url);
    };
    // $scope.SwitchTypes = [ {Name:"Private"}, {Name:"Internal"}, {Name:"External"}];
    $scope.SwitchTypes = [ "Private", "Internal", "External"];

    UpdateVirtualSwitches($scope, $http);
    $scope.EditAdapter = function (adapter) {
        $http.post("/api/editswitch", adapter)
            .success(function(data) {
                $scope.SW = data;
            });
    }

    $scope.SaveNewAdapter = function(adapter) {
        $http.post("/api/newswitch", adapter).success(function(data) {
            $scope.SW = data;
        });
    }

    $scope.NewAdapter = function()
    {
        var adapter = {};
        adapter.Name = "New Virtual Network";
        adapter.Notes = "";
        adapter.SwitchType = "Private";
        adapter.HyperVHost = "localhost";
        $scope.SW.unshift(adapter);

    }
    $scope.log = function(data)
    {
        console.log(data);
    };
}