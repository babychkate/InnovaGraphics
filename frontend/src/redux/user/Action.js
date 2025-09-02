import { toast } from "react-toastify";
import axios from "axios";
import { baseUrl } from "@/config/constants";
import { UPDATE_USER_PROFILE_FAILURE, UPDATE_USER_PROFILE_REQUEST, UPDATE_USER_PROFILE_SUCCESS } from "./ActionType";

export const updateUserProfile = (id, patches) => async (dispatch) => {
    console.log(patches);
    dispatch({ type: UPDATE_USER_PROFILE_REQUEST });

    try {
        const response = await axios.patch(`${baseUrl}/api/User/${id}`, patches, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: UPDATE_USER_PROFILE_SUCCESS, payload: response.data, success: response?.data?.success });
    } catch (e) {
        console.log(e);
        dispatch({ type: UPDATE_USER_PROFILE_FAILURE, payload: e.response?.data?.validationErrors });
    }
}