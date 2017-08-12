import { Component } from '@angular/core';
import { apiService } from '../shared/services/apiService';
import { pDependentTable } from '../shared/model/pDependentTable';
import { pTableMetadata } from '../shared/model/pTableMetadata';
import { scriptsPackage } from '../shared/model/scriptsPackage';
import { sourceForScript } from '../shared/model/sourceForScript';
import { recordsToExtract, tableToExtract } from '../shared/model/recordsToExtract';
import { saveAs } from 'file-saver';

@Component({
    moduleId: module.id,
    selector: 'extractor-component',
    templateUrl: './extractor.component.html'
})
export class ExtractorComponent {
    connectionString: string;
    bpTypeCode: string;
    busy: any;
    script: sourceForScript;

    constructor(private service: apiService) {
        this.script = new sourceForScript();       
        this.script.tablesToProcess = new Array<tableToExtract>();

        this.script.tablesToProcess.push(tableToExtract.createOne('BusinessProcessesTypes', null, true, false, false, true, true));
        this.script.tablesToProcess.push(tableToExtract.createOne('BusinessProcessSteps', null, true, false, false, true, true));
        this.script.tablesToProcess.push(tableToExtract.createOne('BpTypeStepPresentations', null, false, false, false, true, true));
        this.script.tablesToProcess.push(tableToExtract.createOne('Presentations', null, true, false, false, true, true));
        this.script.tablesToProcess.push(tableToExtract.createOne('Scripts', null, true, false, false, true, true));
        this.script.tablesToProcess.push(tableToExtract.createOne('BPTransactionTemplates', null, false, false, false, true, true));
    }

    executePack() {
        let p = new scriptsPackage();
        p.Scripts = new Array<sourceForScript>();
     
        let rootRecord = new recordsToExtract();
        rootRecord.tableName = "BusinessProcessesTypes";
        rootRecord.where = "bptypecode = '" + this.bpTypeCode + "'";

        this.script.scriptName = this.bpTypeCode;
        this.script.rootRecords = new Array<recordsToExtract>();
        this.script.rootRecords.push(rootRecord);
        p.Scripts.push(this.script);

        this.busy = this.service.executePackage(this.connectionString, p).subscribe((b) => {

            saveAs(b, this.bpTypeCode + ".zip");

        });
    }
}