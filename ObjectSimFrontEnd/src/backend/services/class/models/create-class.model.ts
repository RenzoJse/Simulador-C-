export default interface CreateClassModel {
    name: string;
    accessibility: 'Public' | 'Private' | 'Protected';
    isAbstract: boolean;
    isSealed: boolean;
    isOverride: boolean;
    isVirtual: boolean;
    attributes: any[];
    methods: any[];
    parent: string;
}