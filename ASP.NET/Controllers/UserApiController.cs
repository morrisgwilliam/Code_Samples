using Microsoft.AspNet.Identity;
using PortfolioWebApi.Models.Request;
using PortfolioWebApi.Models.Request.Users;
using PortfolioWebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PortfolioWebApi.Controllers
{

    [RoutePrefix("api/users")]
    public class UserApiController : ApiController
    {
        IUserService _userService;
        IJwtAuthService _authService;

        public UserApiController(IUserService userService, IJwtAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [AllowAnonymous]
        [Route("register"), HttpPost]
        public HttpResponseMessage Register(AddUserRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            int id = _userService.AddUser(model);
            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

        [AllowAnonymous]
        [Route("login"), HttpPost]
        public HttpResponseMessage LogIn(UserLogInRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            bool isSuccessful = _userService.LogIn(model);
            var status = HttpStatusCode.BadRequest;
            if (isSuccessful)
            {
                status = HttpStatusCode.OK;
            }
            return Request.CreateResponse(status, isSuccessful);
        }

        [AllowAnonymous]
        [Route("current"), HttpGet]
        public HttpResponseMessage Current()
        {
            var user = _authService.GetCurrentUser();
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [AllowAnonymous]
        [Route("logout"), HttpPost]
        public HttpResponseMessage LogOut()
        {
            _authService.LogOut();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
