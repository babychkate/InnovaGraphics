import { Input } from '@/components/ui/input';
import { Search } from 'lucide-react';
import {
    Select,
    SelectContent,
    SelectGroup,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select";
import React, { useEffect, useRef, useState } from 'react';
import { Button } from '@/components/ui/button';
import { connectToBattleHub, respondToInvitation, sendInvitation } from '@/services/signalRService';
import { useDispatch, useSelector } from 'react-redux';
import InvitationModal from './InvitationModal/InvitationModal';
import TimeoutModal from './TimeoutModal/TimeoutModal';
import { setBattleId, setSelectedTest, startTest } from '@/redux/test/Action';
import { useNavigate } from 'react-router-dom';
import { getImageById } from '@/redux/image/Action';

const UsersComponent = () => {
    const navigate = useNavigate();
    const dispatch = useDispatch();
    const [modalVisible, setModalVisible] = useState(false);
    const [modalMessage, setModalMessage] = useState('');
    const [modalType, setModalType] = useState(null);
    const [inviterUserId, setInviterUserId] = useState(null);

    const owner = useSelector(state => state.auth.user);
    const [users, setUsers] = useState([]);
    const usersRef = useRef([]);


    const selectedTest = useSelector(state => state.test.selectedTest);
    console.log("Selected test in UsersComponent:", selectedTest);

    useEffect(() => {
        if (!owner?.id) return;

        connectToBattleHub(
            owner.id,
            (inviterUser, testId) => {
                dispatch(setSelectedTest({ id: testId }));
                console.log("Отримано запрошення:", inviterUser, testId);
                if (owner.id === inviterUser) {
                    setModalMessage("Почекайте на відповідь...");
                } else {
                    setModalMessage(`Вам надіслано запрошення від ${inviterUser || 'невідомо кого'}`);
                }
                setInviterUserId(inviterUser);
                setModalType("invitation");
                setModalVisible(true);
            },
            () => console.log("User connected:", owner.id),
            () => console.log("User disconnected:", owner.id),
            () => {
                setModalMessage("Час запрошення вичерпано!");
                setModalType("timeout");
                setModalVisible(true);
            },
            (activeUsers) => {
                const mappedUsers = activeUsers
                    .filter(user => user.id !== owner.id)
                    .map(user => ({
                        id: user?.id,
                        email: user?.email,
                        avatarId: user?.profile?.avatarId,
                        avatar: null,
                        userName: user?.userName,
                        level: `Рівень ${Math.floor(Math.random() * 5) + 1}`,
                        status: true
                    }));

                const fetchAvatars = async () => {
                    const updatedUsers = await Promise.all(mappedUsers.map(async (user) => {
                        if (user.avatarId) {
                            try {
                                const avatarBlob = await dispatch(getImageById(user.avatarId)).unwrap();
                                const avatarUrl = URL.createObjectURL(avatarBlob);
                                return { ...user, avatar: avatarUrl };
                            } catch (err) {
                                console.error("Помилка завантаження аватару:", err);
                                return {
                                    ...user,
                                    avatar: "https://cdn-icons-png.flaticon.com/512/149/149071.png",
                                };
                            }
                        }
                        return {
                            ...user,
                            avatar: "https://cdn-icons-png.flaticon.com/512/149/149071.png",
                        };
                    }));

                    setUsers(updatedUsers);
                    usersRef.current = updatedUsers;
                    console.log("Оновлені користувачі:", updatedUsers);
                };

                fetchAvatars();
            },
            () => { },
            (battleId) => {
                console.log("Отримано StartBattle із battleId:", battleId);
                dispatch(setBattleId(battleId));
                console.log(selectedTest);
                console.log("Початок тесту для користувача:", owner.email);
                dispatch(startTest(selectedTest?.id, { userEmail: owner.email }));
                navigate(`/info-planet/1/quiz/${selectedTest?.id}`);
            },
            () => {
                console.log("Отримано PauseTest");
                setModalMessage("Тест призупинено");
                setModalType("pause");
                setModalVisible(true);
            },
            (battleState) => {
                console.log("Отримано showResult:", battleState);
                // setBattleResult(battleState);
            }
        );
    }, [owner?.id, selectedTest, dispatch, navigate]);

    const handleBattleInvite = (targetUser) => {
        sendInvitation(owner.id, targetUser.id, selectedTest?.id);
        setInviterUserId(owner.id);
    };

    const handleAccept = () => {
        console.log(inviterUserId);
        if (!inviterUserId) return;
        respondToInvitation(owner.id, inviterUserId, true);
        setModalVisible(false);
    };

    const handleDecline = () => {
        if (!inviterUserId) return;
        respondToInvitation(owner.id, inviterUserId, false);
        setModalVisible(false);
    };

    const handleClose = () => {
        setModalVisible(false);
    };

    return (
        <>
            {modalType === "invitation" && (
                <InvitationModal
                    open={modalVisible}
                    message={modalMessage}
                    onClose={handleClose}
                    onAccept={handleAccept}
                    onDecline={handleDecline}
                />
            )}
            {(modalType === "timeout" || modalType === "pause") && (
                <TimeoutModal
                    open={modalVisible}
                    message={modalMessage}
                    onClose={handleClose}
                />
            )}

            <div className='min-h-[calc(100vh-121px)] bg-[#FFB57D] flex flex-col gap-4 py-8 px-4'>
                <h1 className='text-2xl font-semibold text-center'>Користувачі програми</h1>
                <div className='flex items-center gap-4'>
                    <div className='relative w-full max-w-sm'>
                        <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-500 w-5 h-5" />
                        <Input className="pl-10 bg-white placeholder-gray-500" placeholder="Шукати за нікнеймом" />
                    </div>
                    <div>
                        <Select>
                            <SelectTrigger className="cursor-pointer bg-white w-[250px]">
                                <SelectValue placeholder="Сортувати за" />
                            </SelectTrigger>
                            <SelectContent>
                                <SelectGroup>
                                    <SelectItem value="nickname">Нікнейм</SelectItem>
                                    <SelectItem value="level">Рівень</SelectItem>
                                </SelectGroup>
                            </SelectContent>
                        </Select>
                    </div>
                </div>
                <div className="flex flex-col gap-4">
                    {users.map((user) => (
                        <div key={user.id} className="grid [grid-template-columns:1fr_0.5fr_2fr_2fr] items-center gap-4 p-4 bg-[#FFDABE] rounded-4xl">
                            <div className="flex items-center gap-4">
                                <img
                                    src={user.avatar}
                                    alt={user.userName}
                                    className="w-14 h-14 rounded-full object-cover"
                                />
                                <div className="text-lg font-medium text-center">{user.userName}</div>
                            </div>
                            <div className="text-gray-600 text-center">{user.level}</div>
                            <div className="text-gray-600 text-center">{user.status ? "Онлайн" : "Офлайн"}</div>
                            <div className="flex justify-end">
                                <Button
                                    onClick={() => handleBattleInvite(user)}
                                    disabled={!user.status || user.id === owner.id}
                                    className="text-sm px-10 py-2 bg-[#E2853E] hover:bg-[#FFB57D] cursor-pointer"
                                >
                                    Викликати на батл
                                </Button>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </>
    );
};

export default UsersComponent;