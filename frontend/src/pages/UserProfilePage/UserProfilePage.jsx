import { Button } from '@/components/ui/button';
import UserPlayer from '@/components/UserPlayer/UserPlayer';
import { getAllPlanets } from '@/redux/planet/Action';
import { ChevronLeft, ChevronRight, Pencil, Plus } from 'lucide-react';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link } from 'react-router-dom';

const UserProfilePage = () => {
    const user = useSelector(state => state.auth.user);
    const dispatch = useDispatch();
    const planets = useSelector(state => state.planet.planets);
    const avatar = useSelector(state => state.image.imageBase64);
    
    useEffect(() => {
        dispatch(getAllPlanets());
    }, [dispatch]);

    const tracks = [
        "Interstellar – Hans Zimmer – Main Theme",
        "Comptine d’un autre été – Yann Tiersen",
        "River Flows in You – Yiruma",
        "Arrival of the Birds – The Cinematic Orchestra",
        "Time – Hans Zimmer"
    ];

    const [currentTrackIndex, setCurrentTrackIndex] = useState(0);

    const handlePrev = () => {
        setCurrentTrackIndex((prevIndex) =>
            prevIndex === 0 ? tracks.length - 1 : prevIndex - 1
        );
    };

    const handleNext = () => {
        setCurrentTrackIndex((prevIndex) =>
            prevIndex === tracks.length - 1 ? 0 : prevIndex + 1
        );
    };

    const lastUnlockedPlanet = planets.filter(p => p.isUnlock && p.id !== "9C781913-9A37-48B1-8091-8047CE3DF862")[0];
    console.log(lastUnlockedPlanet);

    return (
        <div className='pt-[72px]'>
            <div className='flex items-center justify-between py-5 px-30 gap-20'>
                <div className='w-1/2'>
                    <div> { /* Music */}
                        <h2 className="text-2xl text-center font-semibold mb-2">Фонова музика</h2>
                        <div className='flex items-center justify-between p-2 gap-10 rounded-lg shadow w-[500px] mx-auto'>
                            <span
                                onClick={handlePrev}
                                className="cursor-pointer p-2 hover:bg-gray-200 rounded-full"
                            >
                                <ChevronLeft size={24} />
                            </span>
                            <h2 className="text-lg font-medium text-center flex-1 text-ellipsis whitespace-nowrap overflow-hidden">
                                {tracks[currentTrackIndex]}
                            </h2>
                            <span
                                onClick={handleNext}
                                className="cursor-pointer p-2 hover:bg-gray-200 rounded-full"
                            >
                                <ChevronRight size={24} />
                            </span>
                        </div>
                    </div>
                    <div className='h-[50vh] mb-15'> { /* Player */}
                        <UserPlayer />
                        <h2 className="text-2xl text-center mt-2">Векторний джун</h2>
                    </div>
                    <div> { /* Planets theme */}
                        <h2 className="text-[16px] text-center mb-2">Тема програми</h2>
                        <div className='flex items-center justify-between p-2 gap-10 rounded-lg shadow w-[500px] mx-auto'>
                            <span
                                className="cursor-pointer p-2 hover:bg-gray-200 rounded-full"
                            >
                                <ChevronLeft size={24} />
                            </span>
                            <div className='flex items-center justify-between gap-4'>
                                <div className='w-13 h-13'>
                                    <img src="/planet-theme1.png" alt="Planet1" />
                                </div>
                                <div className='w-13 h-13'>
                                    <img src="/planet-theme2.png" alt="Planet2" />
                                </div>
                                <div className='w-13 h-13'>
                                    <img src="/planet-theme3.png" alt="Planet3" />
                                </div>
                                <div className='w-13 h-13'>
                                    <img src="/planet-theme4.png" alt="Planet4" />
                                </div>
                                <div className='w-13 h-13'>
                                    <img src="/planet-theme5.png" alt="Planet5" />
                                </div>
                            </div>
                            <span
                                className="cursor-pointer p-2 hover:bg-gray-200 rounded-full"
                            >
                                <ChevronRight size={24} />
                            </span>
                        </div>
                    </div>
                </div>
                <div className='w-full gap-8 flex flex-col'>
                    <div className='flex items-center justify-center gap-10 border-[rgb(180,180,180)] border-1 py-7 rounded-lg'> { /* User profile details */}
                        <div className='relative'>
                            <img src={avatar || "https://cdn-icons-png.flaticon.com/512/149/149071.png"} alt="Avatar" className='w-36 h-36 rounded-full' />
                            {/* <Button
                                size="icon"
                                variant="ghost"
                                className="absolute cursor-pointer -bottom-1 -right-2 p-1 w-8 h-8 rounded-full hover:bg-gray-100"
                                onClick={() => alert('Змінити аватарку')}
                                title="Змінити фото"
                            >
                                <Pencil size={16} />
                            </Button> */}
                        </div>
                        <div>
                            <h3 className='text-2xl mb-2'>{user?.realName}</h3>
                            <Link to="./updating">
                                <Button size="sm" className="cursor-pointer font-semibold">
                                    Редагувати профіль
                                </Button>
                            </Link>
                        </div>
                    </div>
                    <div className='flex items-center justify-center gap-10 border-[rgb(180,180,180)] border-1 py-7 px-25 rounded-lg flex-wrap'> { /* Planets themes details */}
                        <div className='w-[70px] h-[70px] shrink-0'>
                            <img src="/planet-theme1.png" alt="Planet theme" className='w-full h-full object-contain' />
                        </div>
                        <div className='min-w-0 flex-1'>
                            <h2><span className='underline'>Планета: </span> {lastUnlockedPlanet?.name}</h2>
                            <h3 className='mb-2'><span className='underline'>Рівень: </span> {lastUnlockedPlanet?.number}</h3>
                            <p className='break-words'>{lastUnlockedPlanet?.topic}</p>
                        </div>
                        <div className='flex flex-col gap-4'>
                            <div className='flex items-center justify-center'>
                                <Link to="/shop">
                                    <Button
                                        size="icon"
                                        variant="ghost"
                                        className='flex items-center gap-1 ml-2 text-black px-4 py-2 cursor-pointer rounded-full hover:bg-gray-100'
                                    >
                                        <Plus size={20} />
                                    </Button>
                                </Link>
                                <span className="text-2xl"><img src="/energy.png" alt="Energy" /></span>
                            </div>

                            <div className='flex items-center justify-center gap-2'>
                                <Link >
                                    <Button
                                        size="icon"
                                        variant="ghost"
                                        className='flex items-center gap-1 text-black px-4 py-2 cursor-pointer rounded-full hover:bg-gray-100'
                                    >
                                        <Plus size={20} />
                                    </Button>
                                </Link>
                                <span className="text-2xl"><img src="/coin.png" alt="Coin" /></span>
                            </div>

                            <div className='flex items-center justify-center gap-2'>
                                <Link>
                                    <Button
                                        size="icon"
                                        variant="ghost"
                                        className='flex items-center gap-1 text-black px-4 py-2 cursor-pointer rounded-full hover:bg-gray-100'
                                    >
                                        <Plus size={20} />
                                    </Button>
                                </Link>
                                <span className="text-2xl"><img src="/star.png" alt="Star" /></span>
                            </div>
                        </div>
                    </div>
                    <div className='flex flex-col items-end gap-1'> { /* Links to next pages */}
                        <Link to="./explored-planets">
                            <Button
                                variant="ghost"
                                size="sm"
                                className="cursor-pointer font-semibold"
                            >
                                Досліджені планети →
                            </Button>
                        </Link>
                        <Link to="./certificate">
                            <Button
                                variant="ghost"
                                size="sm"
                                className="cursor-pointer font-semibold"
                            >
                                Переглянути сертифікат →
                            </Button>
                        </Link>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default UserProfilePage;