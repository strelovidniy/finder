interface IUpdateNotificationsConfigRequest {
    enableNotifications: boolean;
    enableUpdateNotifications: boolean;
    enableTagFiltration: boolean;
    filterTags?: string[];
    enableTitleFiltration: boolean;
    filterTitles?: string[];
}

export default IUpdateNotificationsConfigRequest;
