/**
 * Created by onyushkindv on 19.08.2016.
 */

var horoshikiApp = angular.module('horoshikiApp', [
    'ngRoute',
    'ngSanitize',
    'ngResource',
    'ngCookies',
    'layoutControllers',
    'errorControllers',
    'mainControllers',
    'backendServices',
    'accountServices',
    'translationServices'
]);

horoshikiApp.config(['$locationProvider', '$httpProvider', '$routeProvider',
    function($locationProvider, $httpProvider, $routeProvider) {
        $locationProvider.html5Mode(true);
        $locationProvider.hashPrefix('!');

        $routeProvider
            .when('/', {
                templateUrl: '/angular-html/main.view.html',
                controller: 'MainViewController',
                pageId: 'mainViewPage',
                title: 'Главная страница'
            })
            .when('/error', {
                templateUrl: '/angular-html/error.view.html',
                controller: 'ErrorViewController',
                title: 'Что-то пошло не так ...'
            })
            .otherwise({
                redirectTo: '/'
            });
    }
]);

horoshikiApp.run(['$route', '$rootScope', '$location', function ($route, $rootScope, $location) {
    var original = $location.path;
    $location.path = function (path, reload) {
        if (reload === false) {
            var lastRoute = $route.current;
            var un = $rootScope.$on('$locationChangeSuccess', function () {
                $route.current = lastRoute;
                un();
            });
        }
        return original.apply($location, [path]);
    }
}]);

var horoshikiAppLogin = angular.module('horoshikiAppLogin', [
    'ngRoute',
    'ngSanitize',
    'ngResource',
    'ngCookies',
    'layoutControllers',
    'loginControllers',
    'errorControllers',
    'translationServices',
    'backendServices',
    'accountServices'
]);

horoshikiAppLogin.config(['$locationProvider', '$httpProvider', '$routeProvider',
    function($locationProvider, $httpProvider, $routeProvider) {
        // $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        // $httpProvider.defaults.xsrfHeaderName = "X-CSRF-TOKEN";
        $locationProvider.html5Mode(true);
        $locationProvider.hashPrefix('!');

        $routeProvider
            .when('/account/login', {
                templateUrl: '/angular-html/login.view.html',
                controller: 'LoginViewController',
                title: 'Авторизация'
            })
            .otherwise({
                redirectTo: '/account/login'
            });
    }
]);