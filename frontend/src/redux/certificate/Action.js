import { baseUrl } from "@/config/constants";
import { GET_CERTIFICATE_FAILURE, GET_CERTIFICATE_REQUEST, GET_CERTIFICATE_SUCCESS } from "./ActionType"
import axios from "axios";

export const getUserCertificate = (id) => async (dispatch) => {
    dispatch({ type: GET_CERTIFICATE_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Certificate/template-image-url`, {
            headers: {
                'Content-Type': 'application/json'
            },
            params: {
                userId: id,
            },
            withCredentials: true,
        });
        dispatch({ type: GET_CERTIFICATE_SUCCESS, payload: response.data?.data });
    } catch (e) {
        dispatch({ type: GET_CERTIFICATE_FAILURE, payload: e?.response.data?.MarkCount || e?.response.data?.IsCompleted });
    }
}
