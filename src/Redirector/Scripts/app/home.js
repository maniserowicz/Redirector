function LinksCtrl($scope, $http, $timeout) {
    $scope.fetchLinks = function() {
        $http.get('links').success(function (data) {
            $scope.links = data;
        });
    };
    $scope.fetchLinks_delayed = function () {
        $timeout($scope.fetchLinks, 300);
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
            })
            .error(function(exc) {
                alert(exc.Message);
            });
    };
    $scope.clearNew = function() {
        $scope.newLink = null;
    };
    $scope.delete = function (id, evt) {
        evt.preventDefault();
        if (confirm('Really delete link ' + id + '?')) {
            $http.delete('links/' + id)
                .success($scope.fetchLinks);
        }
    };
    
    $scope.fetchLinks();
}