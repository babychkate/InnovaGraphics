import { Button } from '@/components/ui/button';
import { Textarea } from '@/components/ui/textarea';
import { ArrowLeft } from 'lucide-react';
import React from 'react';

const VideoMaterialDetails = ({ video, onBack, topics }) => {
    return (
        <div className="relative min-h-[calc(100vh-200px)] flex items-center justify-center">
            <Button
                variant="ghost"
                onClick={onBack}
                className="absolute -top-2 -left-2 z-20 p-2 rounded-full cursor-pointer transition hover:bg-gray-50"
            >
                <ArrowLeft size={28} />
            </Button>

            <div className="flex items-center justify-between gap-30">
                <div className="flex flex-col items-start gap-4">
                    <h2 className="text-2xl"><span className='font-bold'>Назва:</span> {video?.name}</h2>
                    <p className="text-gray-600 font-medium"><span className='font-bold'>Тип:</span> Відео</p>
                    <div className="text-gray-600 font-medium">
                        <span className='font-bold'>Тема:</span>
                        {Array.isArray(video?.theme) && video.theme.length > 0 ? (
                            <div className="text-sm text-gray-700 list-decimal list-inside flex gap-4">
                                {video.theme.map((topic, index) => (
                                    <span key={index}>{topics?.[topic]}</span>
                                ))}
                            </div>
                        ) : (
                            <div className="text-sm text-gray-700">Тем немає</div>
                        )}
                    </div>
                    <div className="text-gray-600 font-medium flex flex-col">
                        <span className='font-bold'>Посилання на відео:</span>
                        <a
                            href={video?.link}
                            target="_blank"
                            rel="noopener noreferrer"
                            className="text-blue-600 underline"
                        >
                            {video?.link}
                        </a>
                    </div>
                    <div className="text-gray-600 font-medium flex flex-col w-full">
                        <span className='font-bold'>Опис:</span>
                        <Textarea
                            value={video?.description || ''}
                            readOnly
                        />
                    </div>
                </div>
                <div className="h-90 w-90 flex items-center justify-center">
                    <img src={video?.photoPath} alt="Image" />
                </div>
            </div>
        </div>
    );
}

export default VideoMaterialDetails;