import { Button } from '@/components/ui/button';
import React from 'react';

const DeletePlanetModalComponent = ({ planet, onConfirm, onCancel }) => {
    return (
        <div className="fixed inset-0 bg-black/50 flex justify-center items-center z-50">
            <div className="bg-white rounded-lg py-4 px-8 shadow-lg flex flex-col justify-between">
                <div className="flex-1 flex items-center justify-center text-center my-4">
                    <p>
                        Ви впевнені, що хочете видалити планету <strong>{planet.name}</strong>?
                    </p>
                </div>

                <div className="flex justify-end gap-4">
                    <Button
                        onClick={onCancel}
                        className="text-sm bg-[#2354E1] hover:bg-[#2369e1] text-white"
                    >
                        Скасувати
                    </Button>
                    <Button
                        onClick={onConfirm}
                        variant="destructive"
                    >
                        Ок
                    </Button>
                </div>
            </div>
        </div>
    );
};

export default DeletePlanetModalComponent;