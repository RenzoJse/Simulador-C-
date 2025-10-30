import OptionComponent from '../../../../components/dropdown/models/option.component';
export default interface SystemMethodStatus {
    loading?: true;
    systemMethods: Array<OptionComponent>;
    error?: string;
}
