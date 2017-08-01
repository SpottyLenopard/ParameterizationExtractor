import { pFieldMetadata } from './pFieldMetadata';

export class pTableMetadata extends Array<pFieldMetadata> {
    tableName: string;
    uniqueColumnsCollection: string[];
    PK: pFieldMetadata;
}