import axios from "axios";

const register = payload => {
  const config = {
    url: "/api/users/register",
    method: "post",
    data: payload
  };

  return axios(config);
};

const getCurrent = () => {
  const config = {
    url: "/api/users/current",
    method: "get",
    headers: { "Content-Type": "application/json" }
  };

  return axios(config);
};

const logOut = () => {
  const config = {
    url: "/api/users/logout",
    method: "post",
    headers: { "Content-Type": "application/json" }
  };

  return axios(config);
};

const logIn = payload => {
  const config = {
    url: `/api/users/login`,
    method: "post",
    data: payload,
    withCredentials: true,
    crossDomain: true,
    headers: { "Content-Type": "application/json" }
  };
  return axios(config);
};

export { register, getCurrent, logOut, logIn };
