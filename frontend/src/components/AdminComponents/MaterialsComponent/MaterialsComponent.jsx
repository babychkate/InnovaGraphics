import { Button } from '@/components/ui/button';
import React, { useEffect, useState } from 'react';
import VideoMaterialsComponent from './VideoMaterialsComponent/VideoMaterialsComponent';
import { useDispatch, useSelector } from 'react-redux';
import { getAllMaterials, getAllMaterialTypes } from '@/redux/material/Action';

const MaterialsComponent = () => {
    const dispatch = useDispatch();
    const [interfaceOption, setInterfaceOption] = useState("");
    const types = useSelector(state => state.material.types);
    const materials = useSelector(state => state.material.materials);

    useEffect(() => {
        dispatch(getAllMaterialTypes());
        dispatch(getAllMaterials());
    }, [dispatch]);

    console.log(materials);

    const renderContent = () => {
        switch (interfaceOption) {
            case "video-materials":
                return <VideoMaterialsComponent videos={materials.filter(m => m.type == 0)} />
            // Можеш додати інші компоненти тут:
            // case "site-materials":
            //     return <SiteComponent />
            // case "course-materials":
            //     return <CourseComponent />
            default:
                return <div>Виберіть опцію</div>;
        }
    };

    const formatOptionKey = (type) =>
        `${type.toLowerCase()}-materials`;

    const formatLabel = (type) => {
        switch (type) {
            case "Video": return "Відео матеріали";
            case "Site": return "Корисні сайти";
            case "Course": return "Курси";
            default: return type;
        }
    };

    return (
        <div>
            <div className='flex items-center justify-between border-b-1'>
                <div className='flex items-center justify-between'>
                    {Object.entries(types).map(([key, type]) => {
                        const optionKey = formatOptionKey(type);
                        return (
                            <Button
                                key={key}
                                variant="ghost"
                                onClick={() => setInterfaceOption(optionKey)}
                                className={`px-6 py-7 rounded-none border-none shadow-none cursor-pointer focus:ring-0 focus:outline-none 
                                ${interfaceOption === optionKey ? 'bg-[#D6D6D6] text-black' : 'text-muted-foreground'}`}
                            >
                                {formatLabel(type)}
                            </Button>
                        );
                    })}
                </div>
            </div>
            <div className='p-4'>
                {renderContent()}
            </div>
        </div>
    );
};

export default MaterialsComponent;