import React, { useState } from 'react';
import DefaultComponent from './DefaultComponent/DefaultComponent';
import { Button } from '@/components/ui/button';
import { Heart } from 'lucide-react';
import TasksComponent from './TasksComponent/TasksComponent';
import LikedComponent from '../AdditionalInfoPage/LikedComponent/LikedComponent';

const AdditionalTasksPage = () => {
    const [interfaceOption, setInterfaceOption] = useState("");

    const renderContent = () => {
        switch (interfaceOption) {
            case "tasks":
                return <TasksComponent />
            case "liked":
                return <LikedComponent />
            default:
                return <DefaultComponent setInterfaceOption={setInterfaceOption} />
        }
    };

    return (
        <div className='pt-[72px]'>
            <div className='flex items-center justify-between border-b-1'>
                <Button
                    variant="ghost"
                    onClick={() => setInterfaceOption("tasks")}
                    className={`px-6 py-6 rounded-none border-none shadow-none cursor-pointer focus:ring-0 focus:outline-none 
                        ${interfaceOption === "tasks" ? 'bg-[#D6D6D6] text-black' : 'text-muted-foreground'}`}
                >
                    Перелік завдань
                </Button>
                <Button
                    variant="ghost"
                    onClick={() => setInterfaceOption("liked")}
                    className="px-6 py-6 rounded-none border-none shadow-none cursor-pointer focus:ring-0 focus:outline-none"
                >
                    <Heart className="w-15 h-10" />
                </Button>
            </div>
            <div className='w-full'>
                {renderContent()}
            </div>
        </div>
    );
}

export default AdditionalTasksPage;