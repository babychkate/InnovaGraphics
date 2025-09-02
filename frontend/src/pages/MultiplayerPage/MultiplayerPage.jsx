import { Button } from '@/components/ui/button';
import React, { useState } from 'react';
import DefaultMultiplayerComponent from './DefaultMultiplayerComponent/DefaultMultiplayerComponent';
import TestsListComponent from './TestsListComponent/TestsListComponent';
import UsersComponent from './UsersComponent/UsersComponent';
import LidersTableComponent from './LidersTableComponent/LidersTableComponent';
import { useSelector } from 'react-redux';

const MultiplayerPage = () => {
    const [interfaceOption, setInterfaceOption] = useState("");
    const selectedTest = useSelector(state => state.test.selectedTest);

    const handleSelectOpponent = (test) => {
        setInterfaceOption("users");
    };

    const renderContent = () => {
        if (selectedTest && interfaceOption === "users") {
            return <UsersComponent selectedTest={selectedTest} />;
        }

        switch (interfaceOption) {
            case "tests-list":
                return <TestsListComponent onSelectOpponent={handleSelectOpponent} />;
            case "liders-table":
                return <LidersTableComponent />;
            case "users":
                return <UsersComponent />;
            default:
                return <DefaultMultiplayerComponent />;
        }
    };

    return (
        <div className='pt-[72px]'>
            <div className='flex items-center justify-between border-b-1'>
                <div className='flex items-center justify-between'>
                    <Button
                        variant="ghost"
                        onClick={() => setInterfaceOption("tests-list")}
                        className={`px-6 py-6 rounded-none border-none shadow-none cursor-pointer focus:ring-0 focus:outline-none 
                        ${interfaceOption === "tests-list" ? 'bg-[#D6D6D6] text-black' : 'text-muted-foreground'}`}
                    >
                        Перелік тестів
                    </Button>
                    <Button
                        variant="ghost"
                        onClick={() => setInterfaceOption("users")}
                        className={`px-6 py-6 rounded-none border-none shadow-none cursor-pointer focus:ring-0 focus:outline-none 
                        ${interfaceOption === "users" ? 'bg-[#D6D6D6] text-black' : 'text-muted-foreground'}`}
                    >
                        Користувачі
                    </Button>
                    <Button
                        variant="ghost"
                        onClick={() => setInterfaceOption("liders-table")}
                        className={`px-6 py-6 rounded-none border-none shadow-none cursor-pointer focus:ring-0 focus:outline-none 
                        ${interfaceOption === "liders-table" ? 'bg-[#D6D6D6] text-black' : 'text-muted-foreground'}`}
                    >
                        Таблиця лідерів
                    </Button>
                </div>
            </div>
            <div className='w-full'>
                {renderContent()}
            </div>
        </div>
    );
}

export default MultiplayerPage;