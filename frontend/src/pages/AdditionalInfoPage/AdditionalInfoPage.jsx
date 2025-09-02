import React, { useEffect, useState } from 'react';
import { Button } from '@/components/ui/button';
import DefaultAdditionInfoPageComponent from './DefaultAdditionInfoPageComponent/DefaultAdditionInfoPageComponent';
import { Heart } from 'lucide-react';
import VideosComponent from './VideosComponent/VideosComponent';
import UsefulSitesComponent from './UsefulSitesComponent/UsefulSitesComponent';
import SolutionsComponent from './SolutionsComponent/SolutionsComponent';
import CoursesComponent from './CoursesComponent/CoursesComponent';
import LikedComponent from './LikedComponent/LikedComponent';
import { useDispatch, useSelector } from 'react-redux';
import { getAllMaterials, getAllMaterialThemes } from '@/redux/material/Action';

const AdditionalInfoPage = () => {
    const dispatch = useDispatch();
    const topics = useSelector(state => state.material.themes);
    const materials = useSelector(state => state.material.materials);
    const [interfaceOption, setInterfaceOption] = useState("");

    useEffect(() => {
        dispatch(getAllMaterialThemes());
        dispatch(getAllMaterials());
    }, [dispatch]);

    const renderContent = () => {
        switch (interfaceOption) {
            case "videos":
                return <VideosComponent videos={materials.filter(m => m.type == 0)} topics={topics}  />
            case "useful-sites":
                return <UsefulSitesComponent />
            default:
                return <DefaultAdditionInfoPageComponent />
        }
    };

    return (
        <div className='pt-[72px]'>
            <div className='flex items-center justify-between border-b-1'>
                <div className='flex items-center justify-between'>
                    <Button
                        variant="ghost"
                        onClick={() => setInterfaceOption("videos")}
                        className={`px-6 py-6 rounded-none border-none shadow-none cursor-pointer focus:ring-0 focus:outline-none 
                        ${interfaceOption === "videos" ? 'bg-[#D6D6D6] text-black' : 'text-muted-foreground'}`}
                    >
                        Відеоматеріали
                    </Button>
                    <Button
                        variant="ghost"
                        onClick={() => setInterfaceOption("useful-sites")}
                        className={`px-6 py-6 rounded-none border-none shadow-none cursor-pointer focus:ring-0 focus:outline-none 
                        ${interfaceOption === "useful-sites" ? 'bg-[#D6D6D6] text-black' : 'text-muted-foreground'}`}
                    >
                        Корисні сайти
                    </Button>
                </div>
            </div>
            <div className='w-full'>
                {renderContent()}
            </div>
        </div>
    );
}

export default AdditionalInfoPage;