function LinksCtrl($scope, $http) {
    $scope.fetchLinks = function() {
        $http.get('links').success(function (data) {
            $scope.links = data;
        });
    };
    $scope.new = function() {
        alert('not implemented');
    };
    $scope.fetchLinks();
}