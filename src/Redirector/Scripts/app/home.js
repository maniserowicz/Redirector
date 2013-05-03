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
    $scope.save = function () {
        $http.post('links', $scope.newLink)
            .success(function() {
                $scope.clearNew();
                $scope.hideNew();
                $scope.fetchLinks();
            });
    };
    $scope.clearNew = function() {
        $scope.newLink = null;
    };
    
    $scope.fetchLinks();
}