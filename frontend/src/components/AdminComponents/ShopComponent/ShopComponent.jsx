import React, { useState } from 'react';
import { Button } from '@/components/ui/button';
import AvatarsComponent from './AvatarsComponent/AvatarsComponent';

const ShopComponent = () => {
    const [interfaceOption, setInterfaceOption] = useState("");

    const renderContent = () => {
        switch (interfaceOption) {
            case "avatars":
                return <AvatarsComponent />
            default:
                return <div>Виберіть опцію</div>;
        }
    };

    return (
        <div>
            <div className='flex items-center justify-between border-b-1'>
                <div className='flex items-center justify-between'>
                    <Button
                        variant="ghost"
                        onClick={() => setInterfaceOption("app-themes")}
                        className={`px-6 py-7 rounded-none border-none shadow-none cursor-pointer focus:ring-0 focus:outline-none 
                        ${interfaceOption === "app-themes" ? 'bg-[#D6D6D6] text-black' : 'text-muted-foreground'}`}
                    >
                        Фони програми
                    </Button>
                    <Button
                        variant="ghost"
                        onClick={() => setInterfaceOption("avatars")}
                        className={`px-6 py-7 rounded-none border-none shadow-none cursor-pointer focus:ring-0 focus:outline-none 
                        ${interfaceOption === "avatars" ? 'bg-[#D6D6D6] text-black' : 'text-muted-foreground'}`}
                    >
                        Аватари користувача
                    </Button>           
                </div>
            </div>
            <div className='p-4'>
                {renderContent()}
            </div>
        </div>
    );
}

export default ShopComponent;