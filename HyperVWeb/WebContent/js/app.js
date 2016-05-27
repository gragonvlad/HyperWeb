'use strict';

/* App Module */

angular.module('HyperVWeb', ['ui.directives', 'ui.bootstrap', 'plunker', 'ngGrid', 'angular-duration-format'])
    .config(['$routeProvider','$locationProvider', function($routeProvider, $locationProvider) {
    // Activates HTML5 History API for modern browsers and sets the hashbang for legacy browsers
    $locationProvider.html5Mode(true).hashPrefix('!');
    // Sets the application routers
//    $routeProvider.when('/vmlist', {templateUrl: 'partials/vmlist.html', controller: VMListCtrl});
    $routeProvider.when('/main', {templateUrl: 'partials/main.html', controller: MainCtrl, activetab: 'main'});
    $routeProvider.when('/details', {templateUrl: 'partials/details.html', controller: DetailsCtrl});
//    $routeProvider.when('/newvm', {templateUrl: 'partials/newvm.html', controller: NewVmCtrl});
    $routeProvider.when('/switch', {templateUrl: 'partials/switch.html', controller: SwitchCtrl});
    $routeProvider.when('/help', {templateUrl: 'partials/help.html', controller: HelpCtrl, activetab: 'help'});
    $routeProvider.when('/', {templateUrl: 'partials/main.html', controller: MainCtrl, activetab: 'main'});
    $routeProvider.otherwise({redirectTo: '/'});
}]);
