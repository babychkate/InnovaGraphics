import ForLabsComponent from '@/components/AdminComponents/ForLabsComponent/ForLabsComponent';
import MaterialsComponent from '@/components/AdminComponents/MaterialsComponent/MaterialsComponent';
import PlanetsComponent from '@/components/AdminComponents/PlanetsComponent/PlanetsComponent';
import ShopComponent from '@/components/AdminComponents/ShopComponent/ShopComponent';
import UsersComponent from '@/components/AdminComponents/UsersComponent/UsersComponent';
import { Button } from '@/components/ui/button';
import React, { useState } from 'react';

const AdminPage = () => {
    const [selectedSection, setSelectedSection] = useState('users');

    const renderSection = () => {
        switch (selectedSection) {
            case 'planets':
                return <PlanetsComponent />;
            case 'shop':
                return <ShopComponent />;
            case 'for-labs':
                return <ForLabsComponent />;
            case 'additional-materials':
                return <MaterialsComponent />
            default:
                return <div>Оберіть розділ</div>;
        }
    };

    return (
        <div className="pt-[72px] flex h-screen">
            <div className="w-64 border-r border-r-[#D9D9D9]">
                <div className="flex flex-col">
                    <Button
                        variant="ghost"
                        className={`px-5 py-7 cursor-pointer rounded-none text-left w-full border-b border-b-[#D9D9D9] ${selectedSection === 'planets' ? 'bg-[#D9D9D9]' : ''}`}
                        onClick={() => setSelectedSection('planets')}
                    >
                        Планети
                    </Button>
                    <Button
                        variant="ghost"
                        className={`px-5 py-7 cursor-pointer rounded-none text-left w-full border-b border-b-[#D9D9D9] ${selectedSection === 'shop' ? 'bg-[#D9D9D9]' : ''}`}
                        onClick={() => setSelectedSection('shop')}
                    >
                        Магазин
                    </Button>
                    <Button
                        variant="ghost"
                        className={`px-5 py-7 cursor-pointer rounded-none text-left w-full border-b border-b-[#D9D9D9] ${selectedSection === 'additional-materials' ? 'bg-[#D9D9D9]' : ''}`}
                        onClick={() => setSelectedSection('additional-materials')}
                    >
                        Додаткові матеріали
                    </Button>
                    <Button
                        variant="ghost"
                        className={`px-5 py-7 cursor-pointer rounded-none text-left w-ful border-b border-b-[#D9D9D9] ${selectedSection === 'for-labs' ? 'bg-[#D9D9D9]' : ''}`}
                        onClick={() => setSelectedSection('for-labs')}
                    >
                        Для лабораторних робіт
                    </Button>
                </div>
            </div>

            {/* Main Content */}
            <div className="flex-1">
                {renderSection()}
            </div>
        </div>
    );
};

export default AdminPage;