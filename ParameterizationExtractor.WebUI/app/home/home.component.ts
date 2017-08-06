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
    selector: 'home-component',
    templateUrl: './home.component.html'
})
export class HomeComponent {
    tableName: string;
    result: pDependentTable[];
    result2: pTableMetadata;
    connectionString: string;

    constructor(private service: apiService) { }

    getTable() {
        this.service.getDependentTables(this.tableName, this.connectionString)
            .subscribe((d) => this.result = d);
    }

    getMetaTable() {
        this.service.getTableMetaData(this.tableName, this.connectionString)
            .subscribe((d) => this.result2 = d);
    }

    executePack() {
        let p = new scriptsPackage();
        p.Scripts = new Array<sourceForScript>();

        var script = new sourceForScript();
        script.scriptName = "script1";
        script.rootRecords = new Array<recordsToExtract>();

        var rootRecord = new recordsToExtract();
        rootRecord.tableName = "BusinessProcessesTypes";
        rootRecord.where = "bptypecode = '_IMT_SWIFT_103_INC2'";

        script.rootRecords.push(rootRecord);
        p.Scripts.push(script);

        this.service.executePackage(this.connectionString, p).subscribe((b) => {

            saveAs(b, "test.zip");

        });
    }
}