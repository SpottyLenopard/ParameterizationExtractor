import { extractStrategy } from './extractStrategy';
import { sqlBuildStrategy } from './sqlBuildStrategy';


export class recordsToExtract {
    tableName: string;
    where: string;
    processingOrder: number;
}

export class tableToExtract {
    tableName: string;
    uniqueColumns: string[];

    sqlBuildStrategy: sqlBuildStrategy;
    extractStrategy: extractStrategy;
}