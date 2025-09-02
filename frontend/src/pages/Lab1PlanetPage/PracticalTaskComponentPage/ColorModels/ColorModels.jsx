import { getImageById } from '@/redux/image/Action';
import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';

const ColorModels = () => {
    const [selectedFile, setSelectedFile] = useState(null);
    const [originalImageUrl, setOriginalImageUrl] = useState(null);
    const [modifiedImageUrl, setModifiedImageUrl] = useState(null);

    const dispatch = useDispatch();

    const imageBase64 = useSelector(state => state.image.imageBase64);
    const updatedImageBase64 = useSelector(state => state.code.image);

    const handleFileChange = (event) => {
        const file = event.target.files[0];
        setSelectedFile(file);

        const imageUrl = URL.createObjectURL(file);
        setOriginalImageUrl(imageUrl);
        setModifiedImageUrl(imageUrl);

        dispatch(getImageById("8D6AD65E-3D6B-4A88-80D4-9A9C2CD5C656"));
    };

    useEffect(() => {
        return () => {
            if (originalImageUrl) URL.revokeObjectURL(originalImageUrl);
            if (modifiedImageUrl) URL.revokeObjectURL(modifiedImageUrl);
        };
    }, [originalImageUrl, modifiedImageUrl]);

    return (
        <div className="p-4">
            <h2 className="text-lg font-semibold mb-2">Виберіть фото для виконання задачі</h2>
            <input
                type="file"
                accept="image/*"
                onChange={handleFileChange}
                className="block w-full text-sm text-gray-500
                    file:mr-4 file:py-2 file:px-4
                    file:rounded-full file:border-0
                    file:text-sm file:font-semibold
                    file:bg-blue-50 file:text-blue-700
                    hover:file:bg-blue-100"
            />

            {originalImageUrl && (
                <div className="mt-6 grid grid-cols-2 gap-6">
                    <div>
                        <h3 className="text-sm font-medium mb-2">Оригінальне зображення</h3>
                        <img
                            src={originalImageUrl}
                            alt="Оригінал"
                            className="max-w-full max-h-[300px] rounded-lg shadow"
                        />
                    </div>
                    <div>
                        <h3 className="text-sm font-medium mb-2">Змінене зображення</h3>
                        {imageBase64 || updatedImageBase64 ? (
                            <img
                                src={`data:image/png;base64,${updatedImageBase64 ?? imageBase64}`}
                                alt="Змінене"
                                className="max-w-full max-h-[300px] rounded-lg shadow mb-2"
                            />
                        ) : (
                            <p className="text-gray-500">Очікуємо результат...</p>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
};

export default ColorModels;