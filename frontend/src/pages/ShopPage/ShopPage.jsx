import React, { useState } from 'react';
import {
    Select,
    SelectContent,
    SelectGroup,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select";
import { ShoppingCart } from 'lucide-react';
import { Button } from '@/components/ui/button';

import DefaultSwiper from './DefaultSwiper/DefaultSwiper';
import AppTheme from './AppTheme/AppTheme';
import BuyAvatar from './BuyAvatar/BuyAvatar';
import YourPurchases from './YourPurchases/YourPurchases';

const ShopPage = () => {
    const [interfaceOption, setInterfaceOption] = useState("");

    const renderContent = () => {
        switch (interfaceOption) {
            case "app-theme":
                return <AppTheme />;
            case "avatar":
                return <BuyAvatar />;
            case "your-purchases":
                return <YourPurchases />
            default:
                return <DefaultSwiper />;
        }
    };

    return (
        <div className='pt-[72px]'>
            <div className='flex items-center justify-between px-6 py-4 border-b-1'>
                <div className='flex items-center justify-between gap-4'>
                    <Select onValueChange={setInterfaceOption}>
                        <SelectTrigger className="border-none shadow-none focus:ring-0 focus:outline-none">
                            <SelectValue placeholder="Інтерфейс програми" />
                        </SelectTrigger>
                        <SelectContent>
                            <SelectGroup>
                                <SelectItem value="app-theme" className="hover:rounded-none">Фон програми</SelectItem>
                            </SelectGroup>
                        </SelectContent>
                    </Select>
                    <Select onValueChange={setInterfaceOption}>
                        <SelectTrigger className="border-none shadow-none focus:ring-0 focus:outline-none">
                            <SelectValue placeholder="Для Вашого профілю" />
                        </SelectTrigger>
                        <SelectContent>
                            <SelectGroup>
                                <SelectItem value="avatar">Аватар</SelectItem>
                            </SelectGroup>
                        </SelectContent>
                    </Select>
                </div>
                <div className='flex items-center justify-end gap-4'>
                    <div>
                        <Button
                            variant="ghost" className="rounded-full p-2 cursor-pointer"
                            onClick={() => setInterfaceOption("your-purchases")}
                        >
                            <ShoppingCart className="w-10 h-10" />
                        </Button>
                    </div>
                </div>
            </div>
            <div className='w-full'>
                {renderContent()}
            </div>
        </div >
    );
}

export default ShopPage;