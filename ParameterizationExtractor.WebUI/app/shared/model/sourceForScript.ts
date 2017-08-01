import { recordsToExtract, tableToExtract } from './recordsToExtract'

export class sourceForScript {
    order: number;
    scriptName: string;

    rootRecords: recordsToExtract[];
    tablesToProcess: tableToExtract[];
}