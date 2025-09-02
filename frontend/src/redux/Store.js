import { applyMiddleware, combineReducers, legacy_createStore } from "redux";
import { authReducer } from "./auth/Reducer";
import { thunk } from "redux-thunk";
import { planetReducer } from "./planet/Reducer";
import { testReducer } from "./test/Reducer";
import { codeReducer } from "./code/Reducer";
import { theoryReducer } from "./theory/Reducer";
import { questionReducer } from "./question/Reducer";
import { answerReducer } from "./answer/Reducer";
import { imageReducer } from "./image/Reducer";
import { certificateReducer } from "./certificate/Reducer";
import { caseReducer } from "./case/Reducer";
import { userReducer } from "./user/Reducer";
import { materialReducer } from "./material/Reducer";
import { shopItemsReducer } from "./shop_items/Reducer";
import { exerciseReducer } from "./exercise/Reducer";

const rootReducer = combineReducers({
    auth: authReducer,
    planet: planetReducer,
    test: testReducer,
    code: codeReducer,
    theory: theoryReducer,
    question: questionReducer,
    answer: answerReducer,
    image: imageReducer,
    certificate: certificateReducer,
    case: caseReducer,
    user: userReducer,
    material: materialReducer,
    shopItems: shopItemsReducer,
    exercise: exerciseReducer,
});

export const store = legacy_createStore(rootReducer, applyMiddleware(thunk))