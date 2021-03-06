/**
 * Created by onyushkindv on 25.08.2016.
 */

var usersServices = angular.module('usersServices', []);

accountServices.service('UsersService', ['$http', function ($http) {


    this.getAllUsers = function (currentPage, pageSize, sortField, asc) {
        return $http.get(backendServerAddr+'/api/Accounts?pageNo='+currentPage+'&pageSize='+pageSize+'&sortField='+sortField+'&isAscending='+asc, {timeout: backendTimeout, headers: getToken()});
    };

    this.getAllUsersParams = function (currentPage, pageSize, sortField, asc, params) {
        return $http.get(backendServerAddr+'/api/Accounts?pageNo='+currentPage+'&pageSize='+pageSize+'&sortField='+sortField+'&isAscending='+asc+params, {timeout: backendTimeout, headers: getToken()});
    };


    this.getAllUsersWithoutPaginate = function (sortField, asc) {
        return $http.get(backendServerAddr+'/api/Accounts?pageNo=1&pageSize=100&sortField='+sortField+'&isAscending='+asc, {timeout: backendTimeout, headers: getToken()});
    };

    this.getAllManagers = function (sortField, asc) {
        return $http.get(backendServerAddr+'/api/Accounts/allManager?pageNo=1&pageSize=100&sortField='+sortField+'&isAscending='+asc, {timeout: backendTimeout, headers: getToken()});
    };

    this.userAdd = function (user) {
        var resp = $http({
            url: backendServerAddr+'/api/Accounts/Add',
            method: 'POST',
            data: user,
            headers:  getToken(),
        });
        return resp;
    };

    this.userEdit = function (user) {
        var resp = $http({
            url: backendServerAddr+'/api/Accounts/Update',
            method: 'POST',
            data: user,
            headers:  getToken(),
        });
        return resp;
    };
    
    this.userDetete = function (user) {
        var resp = $http({
            url: backendServerAddr+'/api/Accounts/'+user.Id,
            method: 'DELETE',
            data: user,
            headers:  getToken(),
        });
        return resp;
    };

    this.setPassword = function (passwords) {
        var resp = $http({
            url: backendServerAddr+'/api/Accounts/ChangePasswordUser',
            method: 'POST',
            data: passwords,
            headers:  getToken(),
        });
        return resp;
    };

    this.isExistUser = function (userName) {
        var resp = $http({
            url: backendServerAddr+'/api/Accounts/IsExist',
            method: 'POST',
            data: {
                "UserName": userName
            },
            headers:  getToken(),
        });
        return resp;
    };

    this.getUser = function (id) {
        return $http.get(backendServerAddr+'/api/Accounts/'+id, {timeout: backendTimeout, headers: getToken()});
    };


}]);