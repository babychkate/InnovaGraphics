import { Editor } from '@tinymce/tinymce-react';
import React from 'react';

function MyEditor({ value, onChange, readOnly = false }) {
    const handleEditorChange = (content) => {
        if (!readOnly) {
            onChange(content);
        }
    };

    return (
        <Editor
            value={value}
            onEditorChange={handleEditorChange}
            apiKey="jvvcn3szq32dngf70b458on5btj259lvrvzqxvyhzrih042b"
            init={{
                height: 500,
                readonly: readOnly,
                menubar: !readOnly,
                toolbar: readOnly
                    ? false
                    : 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
                plugins: [
                    'anchor', 'autolink', 'charmap', 'codesample', 'emoticons', 'image', 'link', 'lists', 'media',
                    'searchreplace', 'table', 'visualblocks', 'wordcount', 'checklist', 'mediaembed', 'casechange',
                    'formatpainter', 'pageembed', 'a11ychecker', 'tinymcespellchecker', 'permanentpen', 'powerpaste',
                    'advtable', 'advcode', 'editimage', 'advtemplate', 'ai', 'mentions', 'tinycomments',
                    'tableofcontents', 'footnotes', 'mergetags', 'autocorrect', 'typography', 'inlinecss', 'markdown',
                    'importword', 'exportword', 'exportpdf'
                ],
                extended_valid_elements: 'script[src|type|language]',
                valid_children: '+body[script]',
                verify_html: false,
                cleanup: false,
                tinycomments_mode: 'embedded',
                tinycomments_author: 'Author name',
                mergetags_list: [
                    { value: 'First.Name', title: 'First Name' },
                    { value: 'Email', title: 'Email' },
                ],
                ai_request: (request, respondWith) =>
                    respondWith.string(() => Promise.reject('See docs to implement AI Assistant')),
                automatic_uploads: true,
                file_picker_types: 'image',
                file_picker_callback: (callback, value, meta) => {
                    if (meta.filetype === 'image') {
                        const input = document.createElement('input');
                        input.setAttribute('type', 'file');
                        input.setAttribute('accept', 'image/*');
                        input.onchange = function () {
                            const file = input.files[0];
                            const reader = new FileReader();
                            reader.onload = function () {
                                const id = 'blobid' + new Date().getTime();
                                const blobCache = window.tinymce.activeEditor.editorUpload.blobCache;
                                const blobInfo = blobCache.create(id, file, reader.result);
                                blobCache.add(blobInfo);
                                callback(blobInfo.blobUri(), { title: file.name });
                            };
                            reader.readAsDataURL(file);
                        };
                        input.click();
                    }
                },
                setup: (editor) => {
                    if (readOnly) {
                        editor.on('init', () => {
                            editor.getBody().setAttribute('contenteditable', false);
                        });
                    }
                },
            }}

        />
    );
}

export default MyEditor;