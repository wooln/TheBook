
var app = angular.module("app", ['ngCookies', 'ui.router']);

app.config(function ($urlRouterProvider, $stateProvider) {

    $urlRouterProvider.otherwise('/submit/7');

    var submitState = {
        name: 'submit',
        url: '/submit/:group',
        controller: 'submit_controller',
        templateUrl: 'app/submit.html'
    };

    var submitedState = {
        name: 'submited',
        url: '/submited/:group',
        controller: 'submited_controller',
        templateUrl: 'app/submited.html'
    };

    var weeklyReportState = {
        name: 'weeklyReport',
        url: '/weeklyReport/:group',
        controller: 'weekly_report_controller',
        templateUrl: 'app/weekly-report.html'
    };

    var prayItemState = {
        name: 'prayItem',
        url: '/prayItem/:group',
        controller: 'pray_item_controller',
        templateUrl: 'app/pray-item.html'
    };

    $stateProvider.state(submitState);
    $stateProvider.state(submitedState);
    $stateProvider.state(weeklyReportState);
    $stateProvider.state(prayItemState);
});


app.controller("submited_controller",
    function ($scope, $stateParams) {
        $scope.args = $stateParams;
        $scope.tab = 'submited';
    });

app.controller("submit_controller", function ($scope, $cookieStore, $http, $state, $stateParams) {
    $scope.args = $stateParams;
    $scope.tab = 'submit';
    var group = $scope.args.group;

    $scope.record = {
        group: group,
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
                    $state.go('submited', $scope.args);
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

app.controller("weekly_report_controller",
    function ($scope, $cookieStore, $http, $state, $stateParams) {

        $scope.args = $stateParams;
        $scope.tab = 'weeklyReport';
        var group = $stateParams.group;

        $scope.dayDic = {
            'sunday': '日',
            'monday': '一',
            'tuesday': '二',
            'wednesday': '三',
            'thursday': '四',
            'friday': '五',
            'saturday': '六'
        };

        $scope.oficeSet = 0;

        var reload = function () {
            $scope.loading = true;

            var prarams = {
                weekOfficeSet: $scope.oficeSet,
                group: group,
                r: (new Date()).getTime()
            };

            $http.get('DailyRecords', { params: prarams, cache: false })
            .then(
                function (response) {
                    $scope.loading = false;

                    $scope.report = response.data;
                },
                function (response) {
                    $scope.loading = false;
                    alert('提交失败, 通知徐云金维护程序');
                }
            );
        };

        $scope.previous = function () {
            $scope.oficeSet += -1;
            reload();
        };

        $scope.current = function () {
            $scope.oficeSet = 0;
            reload();
        };

        $scope.next = function () {
            $scope.oficeSet += 1;
            reload();
        };

        $scope.current();

        $scope.toggleDetial = function (row, dayKey) {
            if (row.expandDayKey === dayKey) {
                row.expandDayKey = null;
            } else {
                row.expandDayKey = dayKey;
            }

            if (row.expandDayKey) {
                if (dayKey === 'all') {
                    row.details = row.weekRecord;
                } else {
                    row.details = {};
                    row.details[dayKey] = row.weekRecord[dayKey];
                };
            }
            else {
                row.details = null;
            }
        };

        $scope.expandDayKey = null;
        $scope.toggleDayDetail = function (dayKey) {

            if (!$scope.report)
                return;

            if ($scope.expandDayKey === dayKey) {
                $scope.expandDayKey = null;
            } else {
                $scope.expandDayKey = dayKey;
            }

            angular.forEach($scope.report.items,
                function (row) {
                    if ($scope.expandDayKey) {
                        if (dayKey === 'all') {
                            row.details = row.weekRecord;
                        } else {
                            row.details = {};
                            row.details[dayKey] = row.weekRecord[dayKey];
                        };
                        row.expandDayKey = dayKey;
                    }
                    else {
                        row.details = null;
                        row.expandDayKey = null;
                    }
                });
        };
    });


app.controller("pray_item_controller",
    function ($scope, $stateParams) {
        $scope.args = $stateParams;
        $scope.tab = 'prayItem';

        $scope.userPrays = [];

        $scope.add = function () {
            $scope.userPrays.push({});
        };
    });