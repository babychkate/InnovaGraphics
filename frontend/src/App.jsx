import React, { useEffect, useState } from 'react';
import { Navigate, Route, Routes, useLocation } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';

import AuthPage from './pages/AuthPage';
import HomePage from './pages/HomePage';
import Navbar from './components/Navbar/Navbar';
import InfoPlanet from './components/LabsComponents/InfoPlanet/InfoPlanet';
import MatricesInfoPage from './pages/InfoPlanetNavigationPages/LA/MatricesInfoPage';
import MatrixQuizPage from './pages/InfoPlanetNavigationPages/LA/MatrixQuizPage';
import QuizResultPage from './pages/InfoPlanetNavigationPages/LA/QuizResultPage';
import UserProfilePage from './pages/UserProfilePage/UserProfilePage';
import ExploredPlanetsPage from './pages/ExploredPlanetsPage/ExploredPlanetsPage';
import CertificatePage from './pages/CertificatePage/CertificatePage';
import UpdateUserProfilePage from './pages/UpdateUserProfilePage/UpdateUserProfilePage';
import ShopPage from './pages/ShopPage/ShopPage';
import AdditionalInfoPage from './pages/AdditionalInfoPage/AdditionalInfoPage';
import AdditionalTasksPage from './pages/AdditionalTasksPage/AdditionalTasksPage';
import MultiplayerPage from './pages/MultiplayerPage/MultiplayerPage';
import Lab1PlanetPage from './pages/Lab1PlanetPage/Lab1PlanetPage';
import ExercisePage from './pages/Lab1PlanetPage/ExercisePage/ExercisePage';
import PlanetMaterialPage from './pages/Lab1PlanetPage/PlanetMaterialPage/PlanetMaterialPage';
import AboutUsPage from './pages/AboutUsPage/AboutUsPage';
import FAQPage from './pages/FAQPage/FAQPage';
import AdminPage from './pages/AdminPage/AdminPage';
import { ToastContainer } from 'react-toastify';
import VerificationPage from './pages/VerificationPage/VerificationPage';
import ForgotPasswordPage from './pages/ForgotPasswordPage/ForgotPasswordPage';
import ConfirmPasswordPage from './pages/ConfirmPasswordPage/ConfirmPasswordPage';
import { getCurrentUser } from './redux/auth/Action';
import PracticalTaskPage from './pages/Lab1PlanetPage/PracticalTaskPage/PracticalTaskPage';
import PracticalTaskComponentPage from './pages/Lab1PlanetPage/PracticalTaskComponentPage/PracticalTaskComponentPage';
import { connectToBattleHub } from './services/signalRService';
import BackgroundMusic from './components/BackgroundMusic/BackgroundMusic';

const App = () => {
  const dispatch = useDispatch();
  const auth = useSelector(state => state.auth);
  const { isAuthenticated, user, lastAction } = auth;

  const [loading, setLoading] = useState(true);

  useEffect(() => {
    dispatch(getCurrentUser())
      .finally(() => setLoading(false));
  }, [dispatch]);

  const isRegistered = lastAction === "getCurrentUser" ? isAuthenticated && !!user : isAuthenticated;
  const isAdmin = user?.isTeacher;

  if (loading) {
    return <div>Loading...</div>;
  }

  const PrivateRoute = ({ element }) => {
    return isRegistered ? element : <Navigate to="/auth" />;
  };

  const AdminRoute = ({ element }) => {
    return isAdmin ? element : <Navigate to="/" />;
  };

  return (
    <div>
      {isRegistered && <Navbar isAdmin={isAdmin} />}
      <ToastContainer position="top-center" autoClose={3000} />
      <BackgroundMusic />

      <Routes>
        <Route path="/admin" element={<AdminRoute element={<AdminPage />} />} />

        {!isAdmin && (
          <>
            <Route path="/" element={<PrivateRoute element={<HomePage />} />} />
            <Route path="/auth" element={<AuthPage />} />
            <Route path="/auth/verification" element={<VerificationPage />} />
            <Route path="/auth/forgot-password" element={<ForgotPasswordPage />} />
            <Route path="/reset-password" element={<ConfirmPasswordPage />} />

            <Route path="/shop" element={<PrivateRoute element={<ShopPage />} />} />
            <Route path="/additional-info" element={<PrivateRoute element={<AdditionalInfoPage />} />} />
            <Route path="/additional-tasks" element={<PrivateRoute element={<AdditionalTasksPage />} />} />
            <Route path="/multiplayer" element={<PrivateRoute element={<MultiplayerPage />} />} />

            <Route path="/my-profile" element={<PrivateRoute element={<UserProfilePage />} />} />
            <Route path="/my-profile/explored-planets" element={<PrivateRoute element={<ExploredPlanetsPage />} />} />
            <Route path="/my-profile/certificate" element={<PrivateRoute element={<CertificatePage />} />} />
            <Route path="/my-profile/updating" element={<PrivateRoute element={<UpdateUserProfilePage />} />} />

            <Route path="/info-planet" element={<PrivateRoute element={<InfoPlanet />} />} />
            <Route path="/info-planet/:subtopic" element={<PrivateRoute element={<MatricesInfoPage />} />} />
            <Route path="/info-planet/:subtopic/quiz/:quizId" element={<PrivateRoute element={<MatrixQuizPage />} />} />
            <Route path="/info-planet/:subtopic/quiz/:quizId/result" element={<PrivateRoute element={<QuizResultPage />} />} />
            <Route path="/planets/:planetId" element={<PrivateRoute element={<Lab1PlanetPage />} />} />
            <Route path="/planets/:planetId/exercise" element={<PrivateRoute element={<ExercisePage />} />} />
            <Route path="/planets/:planetId/exercise/materials" element={<PrivateRoute element={<PlanetMaterialPage />} />} />
            <Route path="/planets/:planetId/exercise/practical-task" element={<PrivateRoute element={<PracticalTaskPage />} />} />
            <Route path="/planets/:planetId/exercise/practical-task/:taskId" element={<PrivateRoute element={<PracticalTaskComponentPage />} />} />

            <Route path="/about-us" element={<PrivateRoute element={<AboutUsPage />} />} />
            <Route path="/faq" element={<PrivateRoute element={<FAQPage />} />} />
          </>
        )}

        <Route path="*" element={<Navigate to={isAdmin ? "/admin" : "/"} />} />
      </Routes>
    </div>
  );
};

export default App;