export default interface CreateClassModel {
    name: string;
    accessibility: string;
    isAbstract: boolean;
    isSealed: boolean;
    isVirtual: boolean;
    attributes: any[];
    methods: any[];
    parent: string;
}