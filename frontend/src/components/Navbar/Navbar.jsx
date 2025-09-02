import React, { useEffect, useState } from 'react';
import { Avatar, AvatarFallback, AvatarImage } from '../ui/avatar';
import { Button } from '../ui/button';
import { Bell, Menu } from 'lucide-react';
import { Link, useNavigate } from 'react-router-dom';
import { Popover, PopoverContent, PopoverTrigger } from '../ui/popover';
import { useDispatch, useSelector } from 'react-redux';
import { logout } from '@/redux/auth/Action';
import { getImageById } from '@/redux/image/Action';

const Navbar = ({ isAdmin }) => {
    const [isPopoverOpen, setIsPopoverOpen] = useState(false);
    const navigate = useNavigate();
    const dispatch = useDispatch();
    const user = useSelector(state => state.auth.user);
    const image = useSelector(state => state.image.imageBase64);

    const onLogout = () => {
        setIsPopoverOpen(false);
        dispatch(logout());
        navigate("/auth");
    }

    useEffect(() => {
        if (user?.profile?.avatarId) {
            dispatch(getImageById(user?.profile?.avatarId));
        }
    }, []);

    console.log(image);
    console.log(user);

    return (
        <div className="fixed top-0 left-0 w-full z-50 px-6 py-4 bg-black text-white flex items-center justify-between">
            <div className="text-2xl font-bold">
                <a href="/">Innova Graphics</a>
            </div>

            <nav className="flex items-center justify-between gap-4">
                {!isAdmin && (
                    <>
                        <div className="flex items-center space-x-2">
                            <div className="w-5 h-5 rounded-full bg-gradient-to-br from-yellow-500 to-orange-600 flex items-center justify-center text-white font-semibold"></div>
                            <span className="text-white text-sm">3 planets</span>
                        </div>

                        <div className="flex items-center space-x-2">
                            <div className="w-5 h-5 flex items-center justify-center text-white text-lg"><img src="/star.png" alt="Star" /></div>
                            <span className="text-white text-sm">{user?.markCount} stars</span>
                        </div>

                        <div className="flex items-center space-x-2">
                            <div className="w-5 h-5 flex items-center justify-center text-white text-lg"><img src="/coin.png" alt="Coin" /></div>
                            <span className="text-white text-sm">{user?.coinCount} coins</span>
                        </div>

                        <div className="flex items-center space-x-2">
                            <div className="w-5 h-5 flex items-center justify-center text-white text-lg"><img src="/energy.png" alt="Energy" /></div>
                            <span className="text-white text-sm">{user?.energyCount} energy</span>
                        </div>

                        <div className="relative">
                            <Bell className="w-5 h-5 text-white" />
                            <span className="absolute -top-1 -right-1 w-2.5 h-2.5 bg-red-500 rounded-full animate-ping pointer-events-none" />
                            <span className="absolute -top-1 -right-1 w-2.5 h-2.5 bg-red-500 rounded-full" />
                        </div>
                    </>
                )}

                <Link to="/my-profile">
                    <Avatar className="w-10 h-10 ml-4 cursor-pointer">
                        <AvatarImage src={isAdmin ? "/levus.png" : image || "https://cdn-icons-png.flaticon.com/512/149/149071.png"} alt="User" />
                        <AvatarFallback>UG</AvatarFallback>
                    </Avatar>
                </Link>

                <div className="relative">
                    <Popover open={isPopoverOpen} onOpenChange={setIsPopoverOpen}>
                        <PopoverTrigger asChild>
                            <Button variant="ghost" className="text-white">
                                <Menu />
                            </Button>
                        </PopoverTrigger>
                        <PopoverContent className="bg-black text-white p-2 shadow-lg border-none">
                            {!isAdmin && (
                                <>
                                    <div className="space-y-1 flex flex-col gap-1">
                                        <Link to="/" onClick={() => setIsPopoverOpen(false)} className="block w-full text-left hover:bg-white/20 hover:text-white transition-colors rounded px-2 py-1">
                                            На Головну
                                        </Link>
                                        <Link to="/shop" onClick={() => setIsPopoverOpen(false)} className="block w-full text-left hover:bg-white/20 hover:text-white transition-colors rounded px-2 py-1">
                                            Магазин
                                        </Link>
                                        <Link to="/multiplayer" onClick={() => setIsPopoverOpen(false)} className="block w-full text-left hover:bg-white/20 hover:text-white transition-colors rounded px-2 py-1">
                                            Мультиплеєр
                                        </Link>
                                        <Link to="/additional-info" onClick={() => setIsPopoverOpen(false)} className="block w-full text-left hover:bg-white/20 hover:text-white transition-colors rounded px-2 py-1">
                                            Додаткова інформація
                                        </Link>
                                        <Link to="/additional-tasks" onClick={() => setIsPopoverOpen(false)} className="block w-full text-left hover:bg-white/20 hover:text-white transition-colors rounded px-2 py-1">
                                            Додаткові завдання
                                        </Link>
                                    </div>


                                    <div className="my-2 border-t border-white/20"></div>
                                </>
                            )}

                            <div className="space-y-1">
                                {!isAdmin && (
                                    <>
                                        <Link to="/about-us" onClick={() => setIsPopoverOpen(false)} className="block w-full text-left hover:bg-white/20 hover:text-white transition-colors rounded px-2 py-1">
                                            Про нас
                                        </Link>
                                        <Link to="/faq" onClick={() => setIsPopoverOpen(false)} className="block w-full text-left hover:bg-white/20 hover:text-white transition-colors rounded px-2 py-1">
                                            FAQ
                                        </Link>
                                        <Link to="/contacts" onClick={() => setIsPopoverOpen(false)} className="block w-full text-left hover:bg-white/20 hover:text-white transition-colors rounded px-2 py-1">
                                            Контакти
                                        </Link>
                                    </>
                                )}
                                <Button variant="ghost" onClick={() => onLogout()} className="block w-full text-left hover:bg-white/20 hover:text-white transition-colors rounded px-2 py-1">
                                    Вихід
                                </Button>
                            </div>
                        </PopoverContent>
                    </Popover>
                </div>
            </nav>
        </div>
    );
};

export default Navbar;