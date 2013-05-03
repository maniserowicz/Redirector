function LinksCtrl($scope, $http) {
    $scope.fetchLinks = function() {
        $http.get('links').success(function (data) {
            $scope.links = data;
        });
    };
    $scope.new = function () {
        $scope.addingNew = true;
    };
    $scope.hideNew = function() {
        $scope.addingNew = false;
    };
    $scope.save = function() {
        alert('not implemented');
    };
    $scope.fetchLinks();
}