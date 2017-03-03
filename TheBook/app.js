
var app = angular.module("app", ['ngCookies']);
app.controller("index_controller", function ($scope, $cookieStore, $http) {

    $scope.title = "Holy Bible";

    $scope.record = {
        user: $cookieStore.get('myname'),
        recordDate: new Date(),
        content: '',
        chapters: '',
        remark: ''
    };

    $scope.submit = function () {


        var msg = '';
        if (!$scope.record.user.trim()) {
            msg += '姓名 ';
        }
        if (!$scope.record.content.trim()) {
            msg += '读经内容 ';
        }
        if (!$scope.record.chapters && $scope.record.chapters !== 0) {
            msg += '读经章数 ';
        }
        if (msg) {
            alert(msg + '为必填');
            return;
        }

        $cookieStore.put('myname', $scope.record.user);

        $scope.submiting = true;

        $http.post('DailyRecords', $scope.record)
            .then(
                function (response) {
                    $scope.submiting = false;
                    $scope.submited = true;
                },
                function (response) {
                    $scope.submiting = false;
                    alert('提交失败, 通知徐云金维护程序');
                }
            );
    };

    $scope.unSubmit = function () {
        $scope.submited = false;
    }
});