/* Last push by Laura on 5/10/2016 at 5:44PM */
var dashboardModule = angular.module('dashboardApp', [
    'ngAnimate',
    'ngAria',
    'ngCookies',
    'ngMessages',
    'ngResource',
    'ngRoute',
    'ngSanitize',
    'ngTouch',
    'smart-table',
    'highcharts-ng'
])
    .constant('VerticalEnum', {
        0: 'Warehouse Solutions',
        1: 'Merchandising Solutions',
        2: 'Membership Solutions',
        3: 'Distribution Solutions',
        4: 'International Solutions',
        5: 'Ancillary Solutions',
        6: 'eBusiness Solutions',
        7: 'Corporate Solutions'
    })
      .constant('PhaseEnum', {
          0: 'Start Up',
          1: 'Solution Outline',
          2: 'Macro Design',
          3: 'Micro Design',
          4: 'Build and Test',
          5: 'Deploy',
          6: 'Transition & Close'
      })
    .config(function ($routeProvider) {
        $routeProvider

            .when('/EditProject', {
                templateUrl: 'angular/project/views/EditProject.html',
                controller: 'EditProjectCtrl'
            })
            .when('/DashboardCtrl', {
                templateUrl: 'angular/project/views/AllVerticals.html',
                controller: 'AllVerticalsCtrl'
            })
            .when('/OverviewChart',{
                templateUrl: 'angular/project/views/OverviewChart.html',
                controller: 'OverviewChart'
            })
            .when('/AllVerticals', {
               templateUrl: 'angular/project/views/AllVerticals.html',
               controller: 'AllVerticalsCtrl'
           })
            .when('/Welcome', {
                templateUrl: 'angular/project/views/Welcome.html',
                controller: 'welcomeCtrl'
            })
            .when('/Updates', {
                templateUrl: 'angular/project/views/Updates.html',
                controller: 'updateCtrl'
            })
          .when('/ProjectList/:vId', {
              templateUrl: 'angular/project/views/ProjectList.html',
              controller: 'projectListCtrl'
           })
            .when('/ProjectUpdates/:projectId/:projectName', {
                templateUrl: 'angular/project/views/ProjectUpdates.html',
                controller: 'statusUpdatesCtrl'
            })
             //.when('/ProjectUpdates/:projectId/:projectName', {
             //    templateUrl: 'angular/project/views/ProjectUpdates.html',
             //    controller: 'DevelopmentstatusUpdatesCtrl'
             //})
            .when('/ProjectData/:projectId/:projectUpdateId', {
                templateUrl: 'angular/project/views/ProjectData.html',
                controller: 'statusDataCtrl'
            })
            .when('/Search/:projectText', {
                templateUrl: 'angular/project/views/ProjectList.html',
                controller: 'searchCtrl'
            })
      .otherwise({
          redirectTo: '/Welcome'
      });
    })

    .controller('EditProjectCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum', 'PhaseEnum', function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {

        // Left blank and ready for new code!

    }])


    .controller('welcomeCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum', 'PhaseEnum', function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {

        // Left blank and ready for new code!

    }])
    .controller('OverviewChart', ['$scope', '$http', '$routeParams', 'VerticalEnum', 'PhaseEnum', function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {

        // Left blank and ready for new code!

    }])

    .controller('AllVerticalsCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum', 'PhaseEnum', function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {

    }])
    .controller('updateCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum', 'PhaseEnum', function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {

            // Left blank and ready for new code!

        }])
    
    .controller('projectListCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum', 'PhaseEnum', function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {
        console.log($routeParams.vId);
        $scope.progressNow = 12;
        $scope.showError = 0;
        $scope.showNoResults = 0;
        $http({ method: 'GET', url: '../Vertical/GetVerticalProjects/' + $routeParams.vId }).success(function (data)
        {
            $scope.progressNow = 50;
            setInterval(function () { $scope.progressNow++; }, 500);
            console.log(data);
            
            $scope.vId = $routeParams.vId;
            if ($routeParams.vId != -1) {
                // Vertical Enums does not work for -1, need a more solid fix
                $scope.vName = VerticalEnum[$routeParams.vId];
            } else {
                $scope.vName = "Not Assigned Vertical";
            }
            
            $scope.projectList = [];
            var projData, len;
            var projListIter = 0;
            var iteration;
            for (projData = 0; projData < data.length; ++projData) {
                $scope.projectList[++projListIter] = data[projData];
                
                
            }

            $scope.phaseEnum = PhaseEnum;
            $scope.progressNow = 100;
            console.log($scope.phaseEnum);
        }).error(function (data, status, headers, config) {
            $scope.showError = 1;
            $scope.progressNow = 100;
            console.log(status);
            console.log(data);
            console.log(headers);
            console.log(config);
        })
    }])
    //.controller('statusUpdatesCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum','PhaseEnum',function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {
    //    console.log($routeParams.projectId);
    //    $http({ method: 'GET', url: '../ProjectList/GetProjectUpdates/' + $routeParams.projectId }).success(function (data) {
    //        console.log("data from Get Project Updates:" + data);
    //        console.log("Project Update " + $routeParams.projectId);
    //        $scope.statusUpdateList = data;
    //        console.log("StatusUpdateList")
    //        $scope.vId = $scope.statusUpdateList[0].VerticalID;
    //        $scope.vName = VerticalEnum[$scope.vId];
    //        $scope.phaseEnums = PhaseEnum;
    //        $scope.pId = $routeParams.projectId;
    //        $scope.pName = $routeParams.projectName;
    //        $scope.inProgressPhases = [];
    //        $scope.sortType = 'keyName';
    //        $scope.sortReverse = false;
    //        angular.forEach($scope.statusUpdateList, function (value, key) {
    //            console.log("Phase ID: " + $scope.statusUpdateList[key].PhaseID);
    //            this.push($scope.statusUpdateList[key].PhaseID);

    //        }, $scope.inProgressPhases);
    //        console.log($scope.inProgressPhases);

    //    }).error(function (data, status, headers, config) {
    //        console.log(status);
    //        console.log(data);
    //        console.log(headers);
    //        console.log(config);
    //    })
    //}])
    .controller('statusUpdatesCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum','PhaseEnum',function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {
        console.log($routeParams.projectId);
        $http({ method: 'GET', url: '../ProjectList/GetprojectUpdates/' + $routeParams.projectId }).success(function (data) {
            console.log("data from Get Project Updates:" + data);
            console.log("Project Update " + $routeParams.projectId);
            // Code to get the underscores out of the Phase names for display purposes
            //for (var i = 0; i < data.length; i++) {
            //    var UIPhase = data[i].Phase.replace("_", " ");
            //    data[i].Phase = UIPhase;
            //}
            // End code for getting underscores out of the phase name for display purposes
            $scope.ProjectUpdateList = data;
            $scope.vId = data[0].Project.VerticalID;
            $scope.pName = data[0].Project.ProjectName;
           
            if ($scope.vId != -1) {
                // Vertical Enums does not work for -1, need a more solid fix
                $scope.vName = VerticalEnum[$scope.vId];
            } else {
                $scope.vName = "Not Assigned Vertical";
            }
            $scope.pName = $routeParams.projectName;
            var phases = [];
            phases[0] = 'Start Up';
            phases[1] = 'Solution Outline';
            phases[2] = 'Macro Design';
            phases[3] = 'Micro Design';
            phases[4] = 'Build and Test';
            phases[5] = 'Deploy';
            phases[6] = 'Transition & Close'; 
            $scope.phases = phases;

            // CODE FOR BUBBLES BEGIN
            $scope.phaseEnums = PhaseEnum;
            $scope.inProgressPhases = [0,0,0,0,0,0,0];
            
            angular.forEach($scope.ProjectUpdateList, function (value, key) {
                //var currPhase = $scope.ProjectUpdateList[key].Phase;
                //$scope.inProgressPhases[phases.indexOf(currPhase)] = 1;
                $scope.inProgressPhases[$scope.ProjectUpdateList[key].PhaseID] = 1;
            });
            // CODE FOR BUBBLES END
            // Populate drop down menu
            var selectedPhase = [];

            for (var i = 0; i < data.length; i++){
                selectedPhase[i] = phases[data[i].PhaseID];
            }
    
            $scope.selectedPhase = selectedPhase;
            // End drop down menu population
            
            
            var ProjectUpdateLine = []; //  don't think this variable is being used
            
            $scope.SaveEmail = function (i) {
                
                $scope.ProjectUpdateList[i].PhaseID = phases.indexOf($scope.selectedPhase[i]);
                $scope.ProjectUpdateList[i].Phase = $scope.selectedPhase[i];
                $http.post('../ProjectUpdate/UpdatePhase', $scope.ProjectUpdateList[i])
                    .then(function () {
                        console.log('succeeded updating project');
                    });
            }

            // not sure what this code is about
            angular.forEach($scope.ProjectUpdateList, function (value, key) {
                //projectUpdateBody = angular.fromJson(value.Body);
                //tempArr.push(projectUpdateBody[0].PhaseID);
                //$scope.command = [];

                //$scope.ShowCommand = function (i)
                //{
                //    command[i] = true;
                //}
                
            });
            
            console.log($scope.SaveEmail);

        }).error(function (data, status, headers, config) {
            console.log(status);
            console.log(data);
            console.log(headers);
            console.log(config);
        })
    }])
       .controller('statusDataCtrl', ['$scope', '$http', '$routeParams', 'VerticalEnum','PhaseEnum',function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {
            console.log($routeParams.projectId);
            console.log($routeParams.phaseId);
            console.log($routeParams.statusSequence);
            console.log($routeParams.projectUpdateId);
            $http({ method: 'GET', url: '../ProjectList/GetStatusData/'+$routeParams.projectId+"/"+$routeParams.projectUpdateId }).success(function (data)
            {
                console.log(data);
                console.log($routeParams.projectId);
                
                $scope.statusUpdateList = data;
               // $scope.pName = $routeParams.projectName;
                $scope.date = $scope.statusUpdateList[0].RecordDate;
                $scope.dataExtractionId = $scope.statusUpdateList[0].ProjectUpdateID;
                //$scope.phaseEnums = PhaseEnum;
                $scope.vId = data[0].VerticalID;
                $scope.pName = data[0].ProjectName;
                $scope.pId = data[0].ProjectID;
                //$scope.phaseEnums = PhaseEnum;
                if ($scope.vId != -1) {
                    // Vertical Enums does not work for -1, need a more solid fix
                    $scope.vName = VerticalEnum[$scope.vId];
                } else {
                    $scope.vName = "Not Assigned Vertical";
                }
                $scope.phase = PhaseEnum[$scope.statusUpdateList[0].PhaseID];
              //  $scope.pId = $routeParams.projectId;
            }).error(function(data, status, headers, config) {
                console.log(status);
                console.log(data);
                console.log(headers);
                console.log(config);
            });
        }])
    .controller('searchCtrl', ['$scope', '$http', '$routeParams', 'PhaseEnum', function ($scope, $http, $routeParams, VerticalEnum, PhaseEnum) {
        $scope.progressNow = 10;
        $scope.showError = 0;
        $scope.showNoResults = 0;
        $http({ method: 'GET', url: '../ProjectList/GetStatusUpdates' }).success(function (data) {
            $scope.progressNow = 50;
            console.log(data);
            console.log($routeParams.vId);
            
            $scope.sortType = 'projName';
            $scope.sortReverse = false;

            $scope.vName = $routeParams.projectText;
            $scope.projectList = [];
            var projData, len;
            var projListIter = 0;
            for (projData = 0; projData < data.length; ++projData) {
                if (data[projData].ProjectName.search($routeParams.projectText) != -1) {
                    $scope.projectList[++projListIter] = data[projData];
                }
            }
            if ($scope.projectList.length == 0) {
                $scope.showNoResults = 1;
            }
            $scope.phaseEnum = PhaseEnum;
            $scope.progressNow = 100;
            console.log($scope.phaseEnum);
        }).error(function (data, status, headers, config) {
            $scope.showError = 1;
            console.log(status);
            console.log(data);
            console.log(headers);
            console.log(config);
        })
    }])

    

 .controller('TabsDemoCtrl', function ($scope, $window) {
        $scope.tabs = [
          { title: 'Dynamic Title 1', content: 'Dynamic content 1' },
          { title: 'Dynamic Title 2', content: 'Dynamic content 2', disabled: true }
        ];

        $scope.alertMe = function () {
            setTimeout(function () {
                $window.alert('You\'ve selected the alert tab!');
            });
        };

        $scope.model = {
            name: 'Tabs'
        };
    });