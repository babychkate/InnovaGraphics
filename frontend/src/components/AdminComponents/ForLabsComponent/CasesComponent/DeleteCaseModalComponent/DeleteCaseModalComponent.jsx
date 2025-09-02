import { Button } from '@/components/ui/button';
import React from 'react';

const DeleteCaseModalComponent = ({ onConfirm, onCancel }) => {
    return (
        <div className="fixed inset-0 bg-black/50 flex justify-center items-center z-50">
            <div className="bg-white rounded-lg py-4 px-8 shadow-lg flex flex-col justify-between w-full max-w-xl">
                <div className="flex-1 flex items-center justify-center text-center my-4">
                    <p>
                        Ви впевнені, що хочете видалити тестовий сценарій?
                    </p>
                </div>

                <div className="flex justify-end gap-4">
                    <Button
                        onClick={onCancel}
                        className="text-sm bg-[#2354E1] hover:bg-[#2369e1] text-white cursor-pointer"
                    >
                        Скасувати
                    </Button>
                    <Button
                        onClick={onConfirm}
                        variant="destructive"
                        className="cursor-pointer"
                    >
                        Видалити
                    </Button>
                </div>
            </div>
        </div>
    );
};

export default DeleteCaseModalComponent;