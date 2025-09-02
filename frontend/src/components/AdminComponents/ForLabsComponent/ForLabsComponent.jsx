import React, { useState } from 'react';
import { Button } from '@/components/ui/button';
import TestsComponent from './TestsComponent/TestsComponent';
import TheoriesComponent from './TeoriesComponent/TheoriesComponent';
import CasesComponent from './CasesComponent/CasesComponent';

const ForLabsComponent = () => {
    const [interfaceOption, setInterfaceOption] = useState("");

    const renderContent = () => {
        switch (interfaceOption) {
            case "teories-materials":
                return <TheoriesComponent />
            case "cases": 
                return <CasesComponent />
            case "tests":
                return <TestsComponent />;
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
                        onClick={() => setInterfaceOption("teories-materials")}
                        className={`px-6 py-7 rounded-none border-none shadow-none cursor-pointer focus:ring-0 focus:outline-none 
                        ${interfaceOption === "teories-materials" ? 'bg-[#D6D6D6] text-black' : 'text-muted-foreground'}`}
                    >
                        Теоретичий матеріали
                    </Button>
                    <Button
                        variant="ghost"
                        onClick={() => setInterfaceOption("cases")}
                        className={`px-6 py-7 rounded-none border-none shadow-none cursor-pointer focus:ring-0 focus:outline-none 
                        ${interfaceOption === "cases" ? 'bg-[#D6D6D6] text-black' : 'text-muted-foreground'}`}
                    >
                        Тестові сценарії
                    </Button>
                    <Button
                        variant="ghost"
                        onClick={() => setInterfaceOption("tests")}
                        className={`px-6 py-7 rounded-none border-none shadow-none cursor-pointer focus:ring-0 focus:outline-none 
                        ${interfaceOption === "tests" ? 'bg-[#D6D6D6] text-black' : 'text-muted-foreground'}`}
                    >
                        Тести
                    </Button>
                </div>
            </div>
            <div className='p-4'>
                {renderContent()}
            </div>
        </div>
    );
}

export default ForLabsComponent;