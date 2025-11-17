

var app = angular.module('myApp', ["ngRoute", 'ngCookies', 'ngTagsInput', 'ngTable']);


app.config(function ($routeProvider) {

    

    $routeProvider
        .when("/", {
            templateUrl: '/Home/UserManagement',
            controller: 'userCtrl'

        })
        .when("/addUser", {
            templateUrl: '/Home/AddUser',
            controller: 'addUserCtrl'
        })
        .when("/editUser/:userId", {
            templateUrl: '/Home/AddUser',
            controller: 'addUserCtrl'

        })
        .when("/sendEmail", {
            templateUrl: '/Home/SendEmail',
            controller: 'sendEmailCtrl'
        }).when("/profile", {
            templateUrl: '/Account/UserProfile',
            controller: 'profileCtrl'
        })
        
  
});



app.run(function ($rootScope, $cookies, $location) {
    $rootScope.CurrentUser = $cookies.get('UserInfo');

    
    if ($rootScope.CurrentUser == null) {
        
        window.location = "/#!/"
        return;
    }
       

    var res = $rootScope.CurrentUser.split("&");

    $rootScope.username = (res[0]).replace("Username=", "");
    $rootScope.userId = (res[1]).replace("UserId=", "");
    $rootScope.userType = (res[2]).replace("Type=", "");
    $rootScope.userEmail = (res[3]).replace("Email=", "");
    

    //$rootScope.activeItem = 0;
  
    //variables
    $rootScope.isEdit = false;


   


    $rootScope.$on('$locationChangeSuccess', function (event, newUrl, oldUrl) {
        if ($rootScope.CurrentUser == null) {

            window.location = "/#!/"
            return;
        }


        if ($rootScope.userType === "User" &&
            ($location.path() === '/addUser' || $location.path().startsWith('/editUser') || $location.path() === '/')) {
            $location.path('/profile'); 
        }

    })



   
 
})


app.controller('sidebarCtrl', function ($scope, $rootScope, $location) {

    $scope.activeItem = 0;

    $rootScope.setActive = function (item) {
        $scope.activeItem = item;

    }

    $rootScope.$on('$locationChangeSuccess', function (event, newUrl, oldUrl) {
        let url = $location.path();

        if (url === '/') {
            $rootScope.setActive(0);

        }
        

        if (url === '/addUser' || url.startsWith('/editUser/')) {
            $rootScope.setActive(1);

        }
        if (url === '/sendEmail') {
            $rootScope.setActive(2);

        }

        if (url === '/profile') {
            $rootScope.setActive(3);

        }
        
    });
})


app.controller('userCtrl', function ($scope, $http, $timeout, $rootScope, NgTableParams) {
    
  
    $scope.refreshUserData = function()
    {
        $http.get('/User/GetUsers')
            .then((res) => {


                $scope.allUsers = res.data;

                $scope.tableParams.settings({ dataset: $scope.allUsers });

         

            }).catch((rej) => console.log(rej))
    }

    $scope.tableParams = new NgTableParams(
        {
            page: 1,            // start on first page
            count: 10,          // items per page
            filter: {},
            sorting: { Username: "asc" }// initial filter
        }
    );

    $scope.searchBy = 0;


    // on first page load
    $scope.refreshUserData();


    $scope.toastify = function (msg) {
        if (typeof Toastify !== "undefined") {
            Toastify({
                text: msg,
                duration: 3000,
                close: true,
                gravity: "top",
                position: "center",
                backgroundColor: "#4fbe87"
            }).showToast();
        }
    }
    $scope.deleteUser = function (userId) {
            $http.post('/User/DeleteById', { id: userId })
                .then(function (res) {

                
                    $scope.toastify("User deleted successfully!")

                    $scope.refreshUserData();


                })
                .catch(function (err) {
                    console.error(err);
                });
    }

    $scope.acceptUser = function (userId) {
        $http.post('/User/acceptUser', { id: userId, approvedBy: $rootScope.userId })
            .then(function (res) {

         
                $scope.toastify("User accepted successfully!")


                $scope.refreshUserData();
            })
            .catch(function (err) {
                console.error(err);
            });
    }

    $scope.rejectUser = function (userId) {
        $http.post('/User/rejectUser', { id: userId, rejectedBy : $rootScope.userId})
            .then(function (res) {

              
                $scope.toastify("User rejected successfully!")

                $scope.refreshUserData();
            })
            .catch(function (err) {
                console.error(err);
            });
    }

    $scope.unlockUser = function (userId) {
        $http.post('/User/unlockUser', { id: userId })
            .then(function (res) {

           
                $scope.toastify("User unlocked successfully!")

                $scope.refreshUserData();
            })
            .catch(function (err) {
                console.error(err);
            });
    }

    $scope.resetUserPassword = function (userId) {
        $http.post('/User/resetUserPassword', { id: userId })
            .then(function (res) {

        
                $scope.toastify("User password reset successfully!")
                
            })
            .catch(function (err) {
                console.error(err);
            });
    }



    $scope.DoAction = function (actionFn,userId) {
       
       
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Yes',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) actionFn(userId);
        });
      
       
    };

    $scope.ExportUsers_Excel = function () {
        window.location.href = '/User/ExportToExcel';

     

        $scope.toastify("Exporting users...")
    };
    
})


app.controller('addUserCtrl', function ($scope, $http, $timeout, $rootScope, $routeParams) {

    $scope.isAdmin = false;
    $scope.User = {
        CreatedById: $rootScope.userId, FirstName: '', LastName: '',
        FirstNameAr: '', LastNameAr: '', Username: '',
        Email: '', Phone: '', Password: '', Type: 'User', Status: '',
        IsActive: '', IsLocked: '',approvedBy:''
    }
    $scope.confirmPassword = '';
    $scope.passwordMatch = false;
    $scope.updateUserId = $routeParams.userId;

   

    $scope.checkIfUpdate = function () {
        
        if ($scope.updateUserId) {

            $http.get('/User/GetUserById', { params: { id: $scope.updateUserId } })
                .then((res) => {

                    $scope.User = res.data;
                    if ($scope.User.Type === 'Admin')
                        $scope.isAdmin = true;
                    $scope.User.Password = '';
                    $scope.prevUsername = $scope.User.Username;
                }).catch((rej) => console.log(rej))



        }
    }
    $scope.checkIfUpdate();

    $scope.AddUser = function () {




        $http.post('/User/AddUser', $scope.User)
            .then((res) => {


                    Toastify({
                        text: "User added successfully!",
                        duration: 3000,
                        close: true,
                        gravity: "top",
                        position: "center",
                        backgroundColor: "#4fbe87",
                    }).showToast();
                $scope.reset();
            }).catch((rej) => console.log(rej))
    }

    $scope.updateUser = function () {



        $http.post('/User/UpdateUser', $scope.User)
            .then((res) => {

                if (typeof Toastify !== "undefined") {
                    Toastify({
                        text: "User updated successfully!",
                        duration: 3000,
                        close: true,
                        gravity: "top",
                        position: "center",
                        backgroundColor: "#4fbe87"
                    }).showToast();
                }


            }).catch((rej) => console.log(rej))
    }

    $scope.saveUser = function () {

        $scope.userForm.$setSubmitted();
        

        if ($scope.userForm.$invalid || $scope.usernameExists) {

            console.warn("Form invalid");
            return;
        }

        if ($scope.updateUserId) {
            $scope.updateUser();
        } else {
            $scope.AddUser();
        }
    };

    $scope.reset = function () {
        $scope.User = {};
        $scope.confirmPassword = '';
        $scope.isAdmin = false;
        $scope.userForm.$setPristine();
        $scope.userForm.$setUntouched();
    }


    $scope.isUserExists = function () {

        
       
        $http.get('/User/IsUserExists', { params: { Username: $scope.User.Username } })
            .then(function (res) {
                $scope.usernameExists = res.data && $scope.User.Username !== $scope.prevUsername;
                
            })
            .catch(function (err) {
                console.error(err);
            });
    }


    $scope.rejectUser = function (userId) {
        $http.post('/User/rejectUser', { id: userId, rejectedBy: $rootScope.userId })
            .then(function (res) {

                if (typeof Toastify !== "undefined") {
                    Toastify({
                        text: "User rejected successfully!",
                        duration: 3000,
                        close: true,
                        gravity: "top",
                        position: "center",
                        backgroundColor: "#4fbe87"
                    }).showToast();
                }


                window.location.reload();
            })
            .catch(function (err) {
                console.error(err);
            });
    }

    $scope.acceptUser = function (userId) {
        $http.post('/User/acceptUser', { id: userId, approvedBy: $rootScope.userId })
            .then(function (res) {

                if (typeof Toastify !== "undefined") {
                    Toastify({
                        text: "User accepted successfully!",
                        duration: 3000,
                        close: true,
                        gravity: "top",
                        position: "center",
                        backgroundColor: "#4fbe87"
                    }).showToast();
                }


                window.location.reload();
            })
            .catch(function (err) {
                console.error(err);
            });
    }

    $scope.confirmReject = function () {
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Yes',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                $scope.rejectUser($scope.updateUserId);
                window.location.reload();
            } else {

            }
        });
    }

    $scope.confirmApprove = function () {
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Yes',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                $scope.acceptUser($scope.updateUserId);
                window.location.reload();
            } else {

            }
        });
    }

})


app.controller('registerCtrl', function ($scope, $http, $rootScope, $location) {

    $scope.user = {
        CreatedById: null, FirstName: '', LastName: '',
        FirstNameAr: '', LastNameAr: '', Username: '',
        Email: '', Phone: '', Password: '', Type: 'User'
    }
    $scope.confirmPassword = '';



    $scope.step = 1;
    $scope.nextClicked = false;

    $scope.next = function () {
        $scope.nextClicked = true;
        const firstName = ($scope.user.FirstName || '').trim();
        const lastName = ($scope.user.LastName || '').trim();

       
        if (firstName && lastName) {
            $scope.nextClicked = false;
            $scope.step += 1;
        }
       
    }

    $scope.back = function () {
        $scope.step -= 1;
    }


    


    $scope.register = function () {
      
        $scope.regForm.$setSubmitted();
        if ($scope.regForm.$invalid) {

            console.warn("Form invalid");
            return;
        }

        $http.post('/User/Register', $scope.user)
            .then((res) => {
                if (typeof Toastify !== "undefined") {
                    Toastify({
                        text: "User registered successfully!",
                        duration: 3000,
                        close: true,
                        gravity: "top",
                        position: "center",
                        backgroundColor: "#4fbe87"
                    }).showToast();
                }

                window.location = '/#!/';

            }).catch((rej) => console.log(rej))
    }


})

app.controller('profileCtrl', function ($scope, $rootScope, $http) {



    $scope.User = {
        UserId: $rootScope.userId ,
        FirstName: '', LastName: '',FullName : '',FullNameAr: '',
        FirstNameAr: '', LastNameAr: '', Username: '',
        Email: '', Phone: '', Password: ''
    }

    $scope.GetUser = function () {
        $http.get('/User/GetUserById', { params: { id: $rootScope.userId } })
            .then((res) => {

                $scope.User = res.data;
                if ($scope.User.Type === 'Admin')
                    $scope.isAdmin = true;
                $scope.User.Password = '';
            }).catch((rej) => console.log(rej))

    }

    $scope.UpdateProfile = function () {
        $http.post('/User/UpdateProfile', $scope.User)
            .then((res) => {
                if (typeof Toastify !== "undefined") {
                    Toastify({
                        text: "Profile updated successfully!",
                        duration: 3000,
                        close: true,
                        gravity: "top",
                        position: "center",
                        backgroundColor: "#4fbe87"
                    }).showToast();
                }

                window.location.reload();
                
            }).catch((rej) => console.log(rej))

    }


    $scope.GetUser();
   
})

app.controller("sendEmailCtrl", function ($scope,$rootScope,$http) {

    const quill = new Quill('#body', {
        theme: 'snow'
    });

    quill.clipboard.dangerouslyPasteHTML('');


    $scope.email = { From: $rootScope.userEmail, To: [], Subject: '', Body: '' }

    $scope.getAllUsers = function () {
        $http.get('/User/GetUsers')
            .then((res) => {

                $scope.allUsers = res.data;
                console.log($scope.allUsers);
            }).catch((rej) => console.log(rej))

    }
    $scope.getAllUsers();

    $scope.To = [];
  

    $scope.getEmails = function (query) {
        if (!$scope.allUsers) return [];

       
        if (!query) return $scope.allUsers.map(u => ({ text: u.Username }));


      

        return $scope.allUsers
            .filter(u => u.Email.toLowerCase().includes(query.toLowerCase()))
            .map(u => ({ text: u.Email }));
      
    }

    $scope.sendEmail = function () {
        

        $scope.email.To = $scope.To.map(e => e.text)
        $scope.email.Body = quill.root.innerHTML;
        $http.post('/User/SendEmail', $scope.email)
            .then((res) => {

               
                    Toastify({
                        text: "Email sent successfully!",
                        duration: 3000,
                        close: true,
                        gravity: "top",
                        position: "center",
                        backgroundColor: "#4fbe87"
                    }).showToast();
               
            }).catch((rej) => console.log(rej))
    }
})



app.directive("changeColor", function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.on('mouseenter', function () {
                element.css('background-color', 'yellow');
            })

            element.on('mouseleave', function () {
                element.css('background-color', '');
            })
        }
    }
})

app.directive("phoneNumber", function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, element, attrs,ngModel) {

            ngModel.$parsers.push(function (value) {
                var clean = value.replace(/[^0-9+]/g, '')
                if (clean !== value) {
                    ngModel.$setViewValue(clean);
                    ngModel.$render();
                }

                return clean;
            })
        }
    }
})

app.directive("arabic", function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, element, attrs, ngModel) {

            ngModel.$parsers.push(function (value) {
                var clean = value.replace(/[^\u0600-\u06FF\s]/g, '');
                if (clean !== value) {
                    ngModel.$setViewValue(clean);
                    ngModel.$render();
                }

                return clean;
            })
        }
    }
})