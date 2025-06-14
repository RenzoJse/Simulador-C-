import { Component } from '@angular/core';

import { OutputModelService } from "../../../backend/services/outputModel/outputModel.service";

@Component({
    selector: 'app-upload-outputModel',
    templateUrl: 'upload-outputModel.component.html'
})

export class UploadOutputModelComponent {
    constructor(private readonly _outputModelService: OutputModelService) {
    }

    protected atSubmit(uploadedModel: { filename: string; file: File | null }) {
        console.log("hola aca estoy");
        if (!uploadedModel.file) {
            console.error('No file selected for upload.');
            return;
        }

        console.log('File:', uploadedModel);
        this._outputModelService.uploadDllFile(uploadedModel.file).subscribe({
            next: (response) => {
                console.log('Upload successful:', response);
            },
            error: (error) => {
                console.error('Error uploading output model:', error);
            }
        });
    }
}
