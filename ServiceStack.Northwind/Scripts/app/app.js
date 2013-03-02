//var app = angular.module('app', ['ui'])


var app = angular.module('app', ['ui', 'components'], function ($routeProvider, $locationProvider) {
    // configure html5 to get links working on jsfiddle
    // $locationProvider.html5Mode(true);
});

angular.module('components', [])
    .directive('helloWorld', function () {
        return{
            restrict:'E',
            templateUrl:'partials/hello.html',
            scope:{
                name:'@'
            }
        }
    });

app.directive('whenActive', function ($location) {
    return {
        scope:true,
        link:function (scope, element, attrs) {
            scope.$on('$routeChangeSuccess', function () {
                if ($location.path() == element.attr('href')) {
                    element.addClass('active');
                } else {
                    element.removeClass('active');
                }
            });
        }
    };
});

app.value('ui.config', {
    // The ui-jq directive namespace
    jq:{
        // The Tooltip namespace
        tooltip:{
            // Tooltip options. This object will be used as the defaults
            placement:'right'
        }
    }
});

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/', { templateUrl:'partials/home.html', controller:mainCtrl });
    $routeProvider.when('/CustomerSearch', { templateUrl:'partials/CustomerSearch.html', controller:mainCtrl });
    $routeProvider.when('/CustomerEdit', { templateUrl:'partials/CustomerEdit.html', controller:mainCtrl });
    $routeProvider.otherwise({ redirectTo:'/' });

}]);

var mainCtrl = function ($scope, $http) {

    $scope.dto = {};
    $scope.dto.CustomerReqDto = {};
    //$scope.dto.CustomerReqDto.CompanyName = 'a';

    $scope.searchCustomers = function (name) {
        // TODO Test for zero length also
        if (angular.isUndefined(name)) {
            $scope.allCustomers();
            return;
        }
        $http({ method:'POST', url:'/customerSearch', data:{ CompanyName:name }}).
            success(function (data, status, headers, config) {
                $scope.dto.Customers = data;
                toastr.info(data.length + ' Company names beginning with ' + name, '');
            }).
            error(function (data, status, headers, config) {
                toastr.error(data, 'Error');
            });
    };

    $scope.insertCustomer = function () {
        // Create a new customer
        var customer = { CustomerId:"PJS", CompanyName:"Peter J Smith Enterprises" };

        $http({ method:'POST', url:'/customer', data:customer })
            .success(function (data, status, headers, config) {
                toastr.info('Customer saved', '');
            })
            .error(function (data, status, headers, config) {
                toastr.error(data, 'Error');
            });
    };

    $scope.allCustomers = function () {

        $scope.dto.Customers = {};
        $http({ method:'GET', url:'/customers' }).
            success(function (data, status, headers, config) {
                $scope.dto.Customers = data;
                toastr.info('All ' + data.length + ' Customers Retrieved', 'Success');
                console.log($scope.dto);
            }).
            error(function (data, status, headers, config) {
                toastr.error(data, 'Error');
            });

    };


};