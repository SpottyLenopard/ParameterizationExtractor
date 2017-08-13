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

    static createOne(tableName: string, uniqueColumns: string[], throwExecptionIfNotExists: boolean, noInserts: boolean, asIsInserts: boolean, processChildren: boolean, processParents: boolean): tableToExtract {
        let instance = new tableToExtract();
        instance.tableName = tableName;
        instance.uniqueColumns = uniqueColumns;

        let sqlStr = new sqlBuildStrategy();
        sqlStr.asIsInserts = asIsInserts;
        sqlStr.noInserts = noInserts;
        sqlStr.throwExecptionIfNotExists = throwExecptionIfNotExists;

        let extrStr = new extractStrategy();
        extrStr.processChildren = processChildren;
        extrStr.processParents = processParents;
        extrStr.dependencyToExclude = new Array<string>();

        instance.extractStrategy = extrStr;
        instance.sqlBuildStrategy = sqlStr;

        return instance;      
    }
}