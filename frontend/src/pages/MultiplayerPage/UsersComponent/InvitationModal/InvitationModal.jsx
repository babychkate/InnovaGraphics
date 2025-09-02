import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";

const InvitationModal = ({ open, onClose, message, onAccept, onDecline }) => (
    <Dialog open={open} onOpenChange={onClose}>
        <DialogContent className="max-w-sm min-h-[150px]">
            <DialogHeader>
                <DialogTitle>{message}</DialogTitle>
            </DialogHeader>
            <DialogFooter className="flex items-end justify-end gap-2 z-50 relative">
                <Button variant="secondary" onClick={onDecline} className="z-50">Відхилити</Button>
                <Button onClick={onAccept} className="z-50">Прийняти</Button>
            </DialogFooter>
        </DialogContent>
    </Dialog>
);

export default InvitationModal;