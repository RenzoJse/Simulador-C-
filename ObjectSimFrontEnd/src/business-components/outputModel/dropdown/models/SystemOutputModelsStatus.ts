import OptionComponent from '../../../../components/dropdown/models/option.component';

export default interface SystemOutputModelsStatus {
    loading?: true;
    systemModels: Array<OptionComponent>;
    error?: string;
}
