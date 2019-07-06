import React, { useState, useEffect } from "react";
import checkLogin from "./useLoginCheck";
import axios from "axios";
import useSnackbar from "./useSnackbar";

const useResource =  (
) => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(false);
  const setConfig = useSnackbar();
  //const [sendRequest, setSendRequest] = useState(sendRequest)

  const sendRequest = (config) => {
    
    if(config === undefined) return
    if (config.url === null || config.method === null) {
        console.log(`Please provide a method and a url to your request.`);      
        return;
    }
    let compMethod = config.method.toUpperCase()
    if(compMethod === "POST"  && config.payload === null || compMethod === "PUT" && config.payload === null){
        console.log(`Please provide a payload to your ${config.url} request`);
        return;
    }
    // get token
    let tokenKey = checkLogin();

    //set timeout for loading
    let timer = setTimeout(() => {
      setLoading(true);
    }, config.loadingDelay ? config.loadingDelay : 250);

  
    const axiosConfig = {
      url: `${config.url}`,
      method: config.method,
      headers: { "Authorization": `Bearer ${tokenKey}`, ...config.headers },
      payload: config.payload
    };
    axios(axiosConfig)
      .then(r => {
        clearTimeout(timer);
        setLoading(false);
        setData(r.data);
        if(config.successMessage){
          setConfig({
            open: true,
            message: config.successMessage,
            variant: "success"
          })
        }
      })
      .catch(r => {
        debugger;
        clearTimeout(timer);
        setLoading(false);
        // pass error from response to message
        setConfig({
          open: true,
          message: `There was an error processing your request. Please look at the network and console for more information.`,
          variant: "error"
        });
        console.log(r);
      });
  }

  return [sendRequest, data, loading];
};

export default useResource
